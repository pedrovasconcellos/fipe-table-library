# FIPE table Library [en-us]; Biblioteca da tabela FIPE [pt-br];

## Description
### Library for using vehicles data from the FIPE table.
#### License: MIT License
#### Copyright (c) 2020 Pedro Vasconcellos
##### Author: Pedro Henrique Vasconcellos
##### Sponsor: https://vasconcellos.solutions

## Note [en-us]:
- Use the [FipeVehicleTypesEnum] enumerator to say what type of vehicle you want to download for.
- Types of vehicles contained in the enumerator [FipeVehicleTypesEnum] (Car = 1, Motorcycle = 2, TruckAndMicroBus = 3)
- If the (referenceCode == 0), the most current reference will be used, that is, the most current data from the fipe table will be downloaded.
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
- Se o (referenceCode == 0), a referência mais atual será usada, ou seja, os dados mais atuais da tabela fipe serão baixados.
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

## Implementing the library
Example of using the FIPE table library.
```csharp
using Microsoft.Extensions.Logging;
using System;
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
            _logger = new LoggerFactory().CreateLogger("LoggerFIPE");
            _httpRequestSettings = new HttpRequestSettings();
            _httpRequest = new HttpRequest(_logger, _httpRequestSettings);
            _downloadService = new FipeDownloadService(_logger, _httpRequest);
            _normalizedDownloadService = new FipeNormalizedDownloadService(_logger, _downloadService);
            var result = GetExample(FipeVehicleTypesEnum.TruckAndMicroBus, 245);
            Console.WriteLine(result.Vehicles[0]?.Id);
        }

        static NormalizedDownloadResult GetExample(FipeVehicleTypesEnum vehicleType, int referenceCode = 0)
        {
            var result = _normalizedDownloadService.GetDataFromFipeTableByVehicleType(vehicleType, referenceCode);
            if (result.VehicleType == vehicleType && result.ReferenceCode == referenceCode
                && result.Brands.Count > 0 && result.Models.Count > 0 && result.Vehicles.Count > 0)
                return result;
            else
                return null;
        }
    }
}
```

Object to be consumed.
```csharp
using System.Collections.Generic;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads
{
    public class NormalizedDownloadResult
    {
        public int ReferenceCode { get; private set; }
        public FipeVehicleTypesEnum VehicleType { get; set; }
        public IList<FipeVehicleBrand> Brands { get; private set; }
        public IList<FipeVehicleModel> Models { get; private set; }
        public IList<FipeVehicleInformation> Vehicles { get; private set; }
    }
}
```
## Sponsor
[![Vasconcellos Solutions](https://vasconcellos.solutions/assets/open-source/images/company/vasconcellos-solutions-small-icon.jpg)](https://www.vasconcellos.solutions)
### Vasconcellos IT Solutions