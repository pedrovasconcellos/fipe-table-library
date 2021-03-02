using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Enums;
using Vasconcellos.FipeTable.UploadService.Domains.Interfaces;
using Vasconcellos.FipeTable.UploadService.Services.Interfaces;

namespace Vasconcellos.FipeTable.UploadService.Services
{
    public class FipeUploadService : IFipeUploadService
    {
        private readonly ILogger _logger;
        private readonly IFipeDownloadService _downloadService;
        private readonly IFipeNormalizedDownloadService _normalizedDownloadService;
        private readonly IFipeUploadDomain _uploadDomain;

        public FipeUploadService(
            ILogger logger, 
            IFipeDownloadService downloadService, 
            IFipeNormalizedDownloadService normalizedDownloadService,
            IFipeUploadDomain uploadDomain)
        {
            this._logger = logger;
            this._downloadService = downloadService;
            this._normalizedDownloadService = normalizedDownloadService;
            this._uploadDomain = uploadDomain;
        }

        public async Task<bool> Process(bool processingForce)
        {
            var lastFipeReference = _downloadService.GetFipeTableReference();
            if (lastFipeReference == null || lastFipeReference.Id < 1)
            {
                _logger.LogInformation(
                    $"{nameof(lastFipeReference)} invalid.");
                return false;
            }
            int fipeReferenceId = lastFipeReference.Id;

            bool haveReferenceIdGreaterOrEquals = this._uploadDomain
                    .HaveReferenceIdGreaterOrEquals(fipeReferenceId);

            var stopProcessing = !(processingForce || !haveReferenceIdGreaterOrEquals);
            if (stopProcessing)
            {
                _logger.LogInformation(
                    "There is no data to update. The database has the most recent version of the FIPE TABLE.");
                return false;
            }

            var fipeTable = this.GetDataFipeTable(fipeReferenceId);
            var fipeReference = fipeTable.FipeReference;
            var brands = fipeTable.Brands;
            var models = fipeTable.Models;
            var vehicle = fipeTable.Vehicles;
            var prices = fipeTable.Prices;

            await this._uploadDomain.SaveFipeReference(fipeReference);

            if (brands != null && brands.Count > 0)
                await this._uploadDomain.SaveVehicleBrands(brands);

            if (models != null && models.Count > 0)
                await this._uploadDomain.SaveVehicleModels(models);

            if (vehicle != null && vehicle.Count > 0)
                await this._uploadDomain.SaveVehicles(vehicle);

            if (prices != null && prices.Count > 0)
                await this._uploadDomain.SavePrices(prices);

            return true;
        }

        private (FipeReference FipeReference,
            IList<FipeVehicleBrand> Brands, 
            IList<FipeVehicleModel> Models, 
            IList<FipeVehicleInformation> Vehicles, 
            IList<FipeVehiclePrice> Prices)
            GetDataFipeTable(int fipeReferenceId)
        {
            var trucks = this.GetDataFipeTable(FipeVehicleTypesEnum.TruckAndMicroBus, fipeReferenceId);
            var motorcycles = this.GetDataFipeTable(FipeVehicleTypesEnum.Motorcycle, fipeReferenceId);
            var cars = this.GetDataFipeTable(FipeVehicleTypesEnum.Car, fipeReferenceId);

            var referenceIdValid = this.ReferenceIdValid(trucks, motorcycles, cars);
            if (!referenceIdValid)
                throw new ArgumentException($"{nameof(referenceIdValid)}={referenceIdValid}");

            this.LogVehicleNumbers(trucks);
            this.LogVehicleNumbers(motorcycles);
            this.LogVehicleNumbers(cars);

            var brands = this.Join(trucks.Brands, motorcycles.Brands, cars.Brands);
            var models = this.Join(trucks.Models, motorcycles.Models, cars.Models);
            var vehicle = this.Join(trucks.Vehicles, motorcycles.Vehicles, cars.Vehicles);
            var prices = this.Join(trucks.Prices, motorcycles.Prices, cars.Prices);

            this.LogQuantity(nameof(fipeReferenceId), fipeReferenceId);
            this.LogQuantity(nameof(brands), brands.Count);
            this.LogQuantity(nameof(models), models.Count);
            this.LogQuantity(nameof(vehicle), vehicle.Count);
            this.LogQuantity(nameof(prices), prices.Count);

            return (trucks.FipeReference, brands, models, vehicle, prices);
        }

        private NormalizedDownloadResult GetDataFipeTable(FipeVehicleTypesEnum vehicleType, int referenceId)
        {
            var downloadResult = this._normalizedDownloadService
                .GetDataFromFipeTableByVehicleType(vehicleType, referenceId);

            if (downloadResult.VehicleType == vehicleType
                && downloadResult.FipeReference.Id == referenceId
                && downloadResult.Brands.Count > 0
                && downloadResult.Models.Count > 0
                && downloadResult.Vehicles.Count > 0)
                return downloadResult;
            else
                throw new ArgumentNullException($"{nameof(downloadResult)}={downloadResult}");
        }

        private bool ReferenceIdValid(params NormalizedDownloadResult[] paramsDownloadResult)
        {
            int referenceId = paramsDownloadResult?.FirstOrDefault()?.FipeReference?.Id ?? 0;
            if (referenceId == 0)
                return false;

            foreach (var downloadResult in paramsDownloadResult)
            {
                if (downloadResult == null
                    || downloadResult.FipeReference == null
                    || downloadResult.FipeReference.Id != referenceId)
                    return false;
            }
            return true;
        }

        private void LogVehicleNumbers(NormalizedDownloadResult normalizedDownloadResult)
        {
            _logger.LogInformation(
                $"{normalizedDownloadResult?.VehicleType.GetDescription()}={normalizedDownloadResult?.Vehicles?.Count}");
        }

        private IList<T> Join<T>(params IList<T>[] paramsList)
        {
            IList<T> resultList = new List<T>();
            foreach (var list in paramsList)
            {
                resultList.Concat(list);
            }
            return resultList;
        }

        private void LogQuantity(string description, int count)
        {
            _logger.LogInformation($"{description}={count}");
        }
    }
}
