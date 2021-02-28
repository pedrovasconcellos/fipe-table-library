using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        static void Main(string[] args)
        {
            Init();
            
            _logger.LogInformation("Starting Console FIPE TABLE.");

            int lastReferenceCode = _downloadService.GetFipeTableReferenceCode();

            var trucks = GetExample(FipeVehicleTypesEnum.TruckAndMicroBus, lastReferenceCode);
            Task.Run(() => { Save(_logger, trucks); });

            var motorcycles = GetExample(FipeVehicleTypesEnum.Motorcycle, lastReferenceCode);
            Task.Run(() => { Save(_logger, motorcycles); });

            var cars = GetExample(FipeVehicleTypesEnum.Car, lastReferenceCode);
            Save(_logger, cars);

            _logger.LogInformation($"{trucks?.VehicleType.GetDescription()}={trucks?.Vehicles?.Count}");
            _logger.LogInformation($"{motorcycles?.VehicleType.GetDescription()}={motorcycles?.Vehicles?.Count}");
            _logger.LogInformation($"{cars?.VehicleType.GetDescription()}={cars?.Vehicles?.Count}");

            _logger.LogInformation("Finalizing Console FIPE TABLE.");
            Console.ReadKey();
        }

        static void Init()
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
        }


        static NormalizedDownloadResult GetExample(FipeVehicleTypesEnum vehicleType, int referenceCode = 245)
        {
            var downloadResult = _normalizedDownloadService
                .GetDataFromFipeTableByVehicleType(vehicleType, referenceCode);
            
            if (downloadResult.VehicleType == vehicleType 
                && downloadResult.ReferenceCode == referenceCode 
                && downloadResult.Brands.Count > 0 
                && downloadResult.Models.Count > 0 
                && downloadResult.Vehicles.Count > 0)
                return downloadResult;
            else
                throw new ArgumentNullException(nameof(downloadResult));
        }

        private static void Save(ILogger logger, NormalizedDownloadResult entity)
        {
            Save(_logger, entity.Brands).Wait();
            Save(_logger, entity.Models).Wait();
            Save(_logger, entity.Vehicles).Wait();
        }

        static Task Save<T>(ILogger logger, IList<T> entity)
        {
            var mongo = new MongoDBService(_connectionString);
            return mongo.SaveAsync(_logger, entity);
        }
    }
}
