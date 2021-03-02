using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Vasconcellos.FipeTable.DownloadService.Infra;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Enums;
using Vasconcellos.FipeTable.UploadService.Domains;
using Vasconcellos.FipeTable.UploadService.Domains.Interfaces;
using Vasconcellos.FipeTable.UploadService.Repositories;
using Vasconcellos.FipeTable.UploadService.Repositories.Interfaces;
using Vasconcellos.FipeTable.UploadService.Services;
using Vasconcellos.FipeTable.UploadService.Services.Interfaces;

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
        private static IRepository _repository;
        private static IFipeUploadDomain _uploadDomain;
        private static IFipeUploadService _uploadService;

        static void Main(string[] args)
        {
            Init();
            
            _logger.LogInformation("Starting Console FIPE TABLE.");

            _uploadService.Process(false);

            _logger.LogInformation("Finalizing Console FIPE TABLE.");
            Console.ReadKey();
        }

        private static void Init()
        {
            InitConnectionStrig();
            InitILogger();
            InitDownloadService();
            InitUploadService();
        }

        private static void InitConnectionStrig()
        {
            _connectionString = Environment.GetEnvironmentVariable("Vasconcellos.FipeTable.ConsoleApp.MongoDB");

            if (string.IsNullOrEmpty(_connectionString))
                throw new ArgumentException($"The {nameof(_connectionString)} cannot be null or empty");
        }

        private static void InitILogger()
        {
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
        }

        private static void InitDownloadService()
        {
            _httpRequestSettings = new HttpRequestSettings();
            _httpRequest = new HttpRequest(_logger, _httpRequestSettings);
            _downloadService = new FipeDownloadService(_logger, _httpRequest);
            _normalizedDownloadService = new FipeNormalizedDownloadService(_logger, _downloadService);
        }

        private static void InitUploadService()
        {
            _repository = new MongoDBRepository(_logger, _connectionString);
            _uploadDomain = new FipeUploadDomain(_repository);
            _uploadService = new FipeUploadService(_logger, _downloadService, _normalizedDownloadService, _uploadDomain);
        }
    }
}
