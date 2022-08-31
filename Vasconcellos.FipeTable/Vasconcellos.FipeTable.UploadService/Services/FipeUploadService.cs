using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Entities.Denormalized;
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

        /// <summary>
        /// Process the fipe table upload.
        /// Note: If the reference id is parameterized with the value 0, the most current reference will be used, that is, the most current data from the fipe table.
        /// </summary>
        /// <param name="fipeReferenceId"></param>
        /// <param name="processingForce"></param>
        /// <returns>Task<bool></returns>
        public async Task<bool> ProcessUpload(int fipeReferenceId = 0, bool processingForce = false)
        {
            var lastFipeReference = await _downloadService.GetFipeTableReference(fipeReferenceId);
            if (lastFipeReference == null || lastFipeReference.Id < 1)
            {
                _logger.LogInformation(
                    $"{nameof(lastFipeReference)} invalid.");
                return false;
            }
            int fipeReferenceIdSelected = lastFipeReference.Id;

            bool haveReferenceIdGreaterOrEquals = this._uploadDomain
                    .HaveReferenceIdGreaterOrEquals(fipeReferenceIdSelected);

            var stopProcessing = !(processingForce || !haveReferenceIdGreaterOrEquals);
            if (stopProcessing)
            {
                _logger.LogInformation(
                    "There is no data to update. The database has the most recent version of the FIPE TABLE.");
                return false;
            }

            var fipeTable = await this.GetDataFipeTable(fipeReferenceIdSelected);
            var fipeReference = fipeTable.FipeReference;
            var brands = fipeTable.Brands;
            var models = fipeTable.Models;
            var vehicles = fipeTable.Vehicles;
            var prices = fipeTable.Prices;
            var vehiclesDenormalized = fipeTable.VehiclesDenormalized;

            var notSaved = true;
            var retry = 3;
            while (notSaved && retry-- > 0)
            {
                var saved = await this.SaveAll(fipeReference, brands, models, vehicles, prices, vehiclesDenormalized);
                Thread.Sleep(3000);
                notSaved = !saved;
            }

            return true;
        }

        private async Task<bool> SaveAll(FipeReference fipeReference,
            IList<FipeVehicleBrand> brands,
            IList<FipeVehicleModel> models,
            IList<FipeVehicleInformation> vehicles,
            IList<FipeVehiclePrice> prices,
            IList<FipeVehicleInformationDenormalized> vehiclesDenormalized)
        {
            try
            {
                _logger.LogInformation("Starting the data storage process. Please wait for the execution to finish");

                await this._uploadDomain.SaveFipeReference(fipeReference);

                if (brands != null && brands.Count > 0)
                    await this._uploadDomain.SaveVehicleBrands(brands);

                if (models != null && models.Count > 0)
                    await this._uploadDomain.SaveVehicleModels(models);

                if (vehicles != null && vehicles.Count > 0)
                    await this._uploadDomain.SaveVehicles(vehicles);

                if (prices != null && prices.Count > 0)
                    await this._uploadDomain.SavePrices(prices);

                if (vehiclesDenormalized != null && vehiclesDenormalized.Count > 0)
                    await this._uploadDomain.SaveVehiclesDenormalized(vehiclesDenormalized);

                _logger.LogInformation("All data has been saved. Please wait until the execution finishes");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Save ERROR: Message={message}", ex.Message);
                return false;
            }
        }

        private async Task<(FipeReference FipeReference,
            IList<FipeVehicleBrand> Brands, 
            IList<FipeVehicleModel> Models, 
            IList<FipeVehicleInformation> Vehicles, 
            IList<FipeVehiclePrice> Prices,
            IList<FipeVehicleInformationDenormalized> VehiclesDenormalized)>
            GetDataFipeTable(int fipeReferenceId)
        {
            var trucks = await this.GetDataFipeTable(FipeVehicleTypesEnum.TruckAndMicroBus, fipeReferenceId);
            var motorcycles = await this.GetDataFipeTable(FipeVehicleTypesEnum.Motorcycle, fipeReferenceId);
            var cars = await this.GetDataFipeTable(FipeVehicleTypesEnum.Car, fipeReferenceId);

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
            var vehicleDenormalized = this.Join(trucks.VehiclesDenormalized, motorcycles.VehiclesDenormalized, cars.VehiclesDenormalized);

            this.LogQuantity(nameof(fipeReferenceId), fipeReferenceId);
            this.LogQuantity(nameof(brands), brands.Count);
            this.LogQuantity(nameof(models), models.Count);
            this.LogQuantity(nameof(vehicle), vehicle.Count);
            this.LogQuantity(nameof(prices), prices.Count);
            this.LogQuantity(nameof(vehicleDenormalized), vehicleDenormalized.Count);

            return (trucks.FipeReference, brands, models, vehicle, prices, vehicleDenormalized);
        }

        private async Task<NormalizedDownloadResult> GetDataFipeTable(FipeVehicleTypesEnum vehicleType, int referenceId)
        {
            var downloadResult = await this._normalizedDownloadService
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
                foreach (var obj in list)
                {
                    resultList.Add(obj);
                }
            }
            return resultList;
        }

        private void LogQuantity(string description, int count)
        {
            _logger.LogInformation($"{description}={count}");
        }
    }
}
