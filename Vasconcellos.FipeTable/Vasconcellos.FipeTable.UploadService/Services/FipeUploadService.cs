using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
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

            var motorcycles = GetExample(FipeVehicleTypesEnum.Motorcycle, fipeReferenceId);
            var trucks = GetExample(FipeVehicleTypesEnum.TruckAndMicroBus, fipeReferenceId);
            var cars = GetExample(FipeVehicleTypesEnum.Car, fipeReferenceId);

            _logger.LogInformation(
                $"{motorcycles?.VehicleType.GetDescription()}={motorcycles?.Vehicles?.Count}");

            _logger.LogInformation(
                $"{trucks?.VehicleType.GetDescription()}={trucks?.Vehicles?.Count}");

            _logger.LogInformation(
                $"{cars?.VehicleType.GetDescription()}={cars?.Vehicles?.Count}");

            var brands = motorcycles.Brands
                .Concat(trucks.Brands)
                .Concat(cars.Brands)
                .ToList();

            var models = motorcycles.Models
                .Concat(trucks.Models)
                .Concat(cars.Models)
                .ToList();

            var vehicle = motorcycles.Vehicles
                .Concat(trucks.Vehicles)
                .Concat(cars.Vehicles)
                .ToList();

            var prices = motorcycles.Prices
                .Concat(trucks.Prices)
                .Concat(cars.Prices)
                .ToList();

            _logger.LogInformation($"fipeReference={fipeReferenceId}");
            _logger.LogInformation($"{nameof(brands)}={brands.Count}");
            _logger.LogInformation($"{nameof(models)}={models.Count}");
            _logger.LogInformation($"{nameof(vehicle)}={vehicle.Count}");
            _logger.LogInformation($"{nameof(prices)}={prices.Count}");

            await this._uploadDomain.SaveFipeReference(motorcycles.FipeReference);

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

        private NormalizedDownloadResult GetExample(FipeVehicleTypesEnum vehicleType, int referenceId = 245)
        {
            var downloadResult = _normalizedDownloadService
                .GetDataFromFipeTableByVehicleType(vehicleType, referenceId);

            if (downloadResult.VehicleType == vehicleType
                && downloadResult.FipeReference.Id == referenceId
                && downloadResult.Brands.Count > 0
                && downloadResult.Models.Count > 0
                && downloadResult.Vehicles.Count > 0)
                return downloadResult;
            else
                throw new ArgumentNullException(nameof(downloadResult));
        }
    }
}
