# FIPE table Library [en-us]; Biblioteca da tabela FIPE [pt-br];

## Description
### Library for using vehicles data from the FIPE table.
#### Version 4.4.0.
#### License: MIT License
#### Copyright (c) 2022 Pedro Vasconcellos
##### Author: Pedro Henrique Vasconcellos
##### Sponsor: https://vasconcellos.solutions

## Note [en-us]:
- Use the [FipeVehicleTypesEnum] enumerator to say what type of vehicle you want to download for.
- Types of vehicles contained in the enumerator [FipeVehicleTypesEnum] (Car = 1, Motorcycle = 2, TruckAndMicroBus = 3)
- If the (referenceId == 0), the most current reference will be used, that is, the most current data from the fipe table will be downloaded.
- Warning: Do not call more than one FIPE library download service method in parallel, as the Proxy FIPE will block your IP for a certain time (5-10 minutes maybe).
- If you want to create the tables in the relational database [MSSQL / SqlServer], there are SQL scripts in the directory=[../Vasconcellos.FipeTable.Database/Tables/].
- If you to view [Vehicle.Year = 32000], it means that this vehicle is a Zero KM vehicle (New).
- The slowing in the download is caused that of the proxy of the [FIPE WebAPI] that performs momentary blocks.
- To avoid making requests while the proxy is locked, the service will pause the task for a few minutes and after will perform normal.
- The truck download is the fastest among them.
- Downloading the three types of vehicles together [Car, Motorcycle, Truck/MicroBus] takes between 2 or 4 hours

## Observações [pt-br]:
- Use o enumerador [FipeVehicleTypesEnum] para dizer qual o tipo de veículo você deseja realizar o downlad.
- Tipos de veículos contidos no enumerador [FipeVehicleTypesEnum] (Carro = 1, Motocicleta = 2, Caminhão e Micro-Ônibus = 3)
- Se o (referenceId == 0), a referência mais atual será usada, ou seja, os dados mais atuais da tabela fipe serão baixados.
- Aviso: não chame mais de um método de serviço de download da biblioteca FIPE em paralelo, pois o Proxy da FIPE bloqueará o seu IP por um determinado tempo (talvez de 5 a 10 minutos).
- Se você deseja criar as tabelas no banco de dados relacional [MSSQL / SqlServer], existem scripts SQL no diretório = [../Vasconcellos.FipeTable.Database/Tables/].
- Se você visualizar [Vehicle.Year = 32000], quer dizer que este veículo, é um veículo Zero KM (Novo).
- O lentidão no download é causado pelo proxy da [FIPE WebAPI], a qual executa bloqueios momentâneos.
- Para evitar de fazer solicitações enquanto o proxy estiver bloqueado, o serviço pausará a tarefa por alguns minutos e depois será executado normalmente.
- O download do caminhão é o mais rápido entre eles.
- O download dos três tipos de veículos juntos [Carro, Motocicleta, Caminhão / MicroBus] leva entre 2 ou 4 horas

## Library to download the data from the fipe table through FIPE WebAPI.
- Nuget: https://www.nuget.org/packages/Vasconcellos.FipeTable.DownloadService
- Nuget .NET CLI: dotnet add package Vasconcellos.FipeTable.DownloadService

## Library with the entities for the use of vehicle data from the FIPE table.
- Nuget: https://www.nuget.org/packages/Vasconcellos.FipeTable.Types
- Nuget .NET CLI: dotnet add package Vasconcellos.FipeTable.Types

## Library to upload the data from the fipe table in the database through FIPE Web API. Save the data from the fipe table on Mongodb
- Nuget: https://www.nuget.org/packages/Vasconcellos.FipeTable.UploadService
- Nuget .NET CLI: dotnet add package Vasconcellos.FipeTable.UploadService

## Implementing the library
Example of using the FipeTable.DownloadService Library.
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Infra;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Enums;

namespace ConsoleApp
{
    public class Program
    {
        private static ILogger _logger;
        private static IHttpRequestSettings _httpRequestSettings;
        private static IHttpRequest _httpRequest;
        private static IFipeDownloadService _downloadService;
        private static IFipeNormalizedDownloadService _normalizedDownloadService;

        static void Main(string[] args)
        {
            Init();

            _logger.LogDebug("Starting Console FIPE TABLE.");

            var lastFipeReference = _downloadService.GetFipeTableReference();
            var result = GetExample(FipeVehicleTypesEnum.TruckAndMicroBus, lastFipeReference.Id);

            _logger.LogDebug(result.Vehicles[0]?.Id);
            _logger.LogDebug("Finalizing Console FIPE TABLE.");
        }

        static void Init()
        {
            using var serviceProvider = new ServiceCollection()
                .AddLogging(config =>
                    config
                        .ClearProviders()
                        .AddConsole()
                        .SetMinimumLevel(LogLevel.Trace)
                    )
                .BuildServiceProvider();

            _logger = serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            _httpRequestSettings = new HttpRequestSettings();
            _httpRequest = new HttpRequest(_logger, _httpRequestSettings);
            _downloadService = new FipeDownloadService(_logger, _httpRequest);
            _normalizedDownloadService = new FipeNormalizedDownloadService(_logger, _downloadService);
        }


        static NormalizedDownloadResult GetExample(FipeVehicleTypesEnum vehicleType, int referenceId)
        {
            var downloadResult = _normalizedDownloadService
                .GetDataFromFipeTableByVehicleType(vehicleType, referenceId);
            
            if (downloadResult.VehicleType == vehicleType 
                && downloadResult.FipeReference.Id == referenceId 
                && downloadResult.Brands.Count > 0 
                && downloadResult.Models.Count > 0 
                && downloadResult.Vehicles.Count > 0 
                && downloadResult.Prices.Count > 0)
                return downloadResult;
            else
                throw new ArgumentNullException(nameof(downloadResult));
        }
    }
}
```

Example of using the FipeTable.UploadService Library.
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Vasconcellos.FipeTable.DownloadService.Infra;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
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

            _uploadService.ProcessUpload().Wait();

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
```
## Sponsor
[![Vasconcellos Solutions](https://vasconcellos.solutions/assets/open-source/images/company/vasconcellos-solutions-small-icon.jpg)](https://www.vasconcellos.solutions)
### Vasconcellos IT Solutions