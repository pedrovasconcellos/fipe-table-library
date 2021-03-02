using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Vasconcellos.FipeTable.ConsoleApp.Services;
using Vasconcellos.FipeTable.DownloadService.Infra;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.ConsoleApp
{
    public class Program
    {
        private static ILogger _logger;
        private static IHttpRequestSettings _httpRequestSettings;
        private static IHttpRequest _httpRequest;
        private static IFipeDownloadService _downloadService;
        private static IFipeNormalizedDownloadService _normalizedDownloadService;
        private static string _connectionString;
        private static MongoDBService _mongo;

        static void Main(string[] args)
        {
            Init();
            
            _logger.LogInformation("Starting Console FIPE TABLE.");

            var lastFipeReference = _downloadService.GetFipeTableReference();
            
            var haveReferenceIdGreaterOrEquals = _mongo
                .HaveReferenceIdGreaterOrEquals(_logger, lastFipeReference.Id);

            if (!haveReferenceIdGreaterOrEquals)
                Process(lastFipeReference.Id);
            else
                _logger.LogInformation(
                    "There is no data to update. The database has the most recent version of the FIPE TABLE.");

            _logger.LogInformation("Finalizing Console FIPE TABLE.");
            Console.ReadKey();
        }

        private static void Init()
        {
            _connectionString = Environment.GetEnvironmentVariable("Vasconcellos.FipeTable.ConsoleApp.MongoDB");
            
            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentException($"The {nameof(_connectionString)} cannot be null or empty");

            using var serviceProvider = new ServiceCollection()
                .AddLogging(config =>
                    config
                        .ClearProviders()
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Debug)
                    )
                .BuildServiceProvider();

            _logger = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            if (_logger is null)
                throw new ArgumentNullException(nameof(_logger));

            _httpRequestSettings = new HttpRequestSettings();
            _httpRequest = new HttpRequest(_logger, _httpRequestSettings);
            _downloadService = new FipeDownloadService(_logger, _httpRequest);
            _normalizedDownloadService = new FipeNormalizedDownloadService(_logger, _downloadService);
            _mongo = new MongoDBService(_connectionString);
        }

        private static void Process(int lastReferenceId)
        {
            var motorcycles = GetExample(FipeVehicleTypesEnum.Motorcycle, lastReferenceId);
            var trucks = GetExample(FipeVehicleTypesEnum.TruckAndMicroBus, lastReferenceId);
            var cars = GetExample(FipeVehicleTypesEnum.Car, lastReferenceId);

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

            _mongo.SaveFipeReference(_logger, motorcycles.FipeReference).Wait();
            _mongo.SaveVehicleBrands(_logger , brands).Wait();
            _mongo.SaveVehicleModels(_logger , models).Wait();
            _mongo.SaveVehicles(_logger, vehicle).Wait();
            _mongo.SavePrices(_logger, prices).Wait();

            _logger.LogInformation($"{nameof(brands)}={brands.Count}");
            _logger.LogInformation($"{nameof(models)}={models.Count}");
            _logger.LogInformation($"{nameof(vehicle)}={vehicle.Count}");
        }

        private static NormalizedDownloadResult GetExample(FipeVehicleTypesEnum vehicleType, int referenceId = 245)
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
