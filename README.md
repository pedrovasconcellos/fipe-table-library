# FIPE table Library [en-us]; Biblioteca da tabela FIPE [pt-br];

### Description: Library for using vehicles data from the FIPE table.

#### License: MIT License
#### Copyright (c) 2020 Pedro Vasconcellos
##### Author: Pedro Henrique Vasconcellos
##### Site: https://vasconcellos.solutions

#### Note [en-us].
- Use the [FipeVehicleTypesEnum] enumerator to say what type of vehicle you want to download for.
- Types of vehicles contained in the enumerator [FipeVehicleTypesEnum] (Car = 1, Motorcycle = 2, TruckAndMicroBus = 3)
- If the (referenceCode == 0), the most current reference will be used, that is, the most current data from the fipe table will be downloaded.
- Warning: Do not call more than one FIPE library download service method in parallel, as the Proxy FIPE will block your IP for a certain time (5-10 minutes maybe).
- If you want to create the tables in the relational database [MSSQL / SqlServer], there are SQL scripts in the directory=[../Vasconcellos.FipeTable.Database/Tables/].
- If you to view [Vehicle.Year = 32000], it means that this vehicle is a Zero KM vehicle (New).

#### Observações [pt-br]:
- Use o enumerador [FipeVehicleTypesEnum] para dizer qual o tipo de veículo você deseja realizar o downlad.
- Tipos de veículos contidos no enumerador [FipeVehicleTypesEnum] (Carro = 1, Motocicleta = 2, Caminhão e Micro-Ônibus = 3)
- Se o (referenceCode == 0), a referência mais atual será usada, ou seja, os dados mais atuais da tabela fipe serão baixados.
- Aviso: não chame mais de um método de serviço de download da biblioteca FIPE em paralelo, pois o Proxy da FIPE bloqueará o seu IP por um determinado tempo (talvez de 5 a 10 minutos).
- Se você deseja criar as tabelas no banco de dados relacional [MSSQL / SqlServer], existem scripts SQL no diretório = [../Vasconcellos.FipeTable.Database/Tables/].
- Se você visualizar [Vehicle.Year = 32000], quer dizer que este veículo, é um veículo Zero KM (Novo).

#### Library to download the data from the fipe table through FIPE WebAPI.
- Nuget: https://www.nuget.org/packages/Vasconcellos.FipeTable.DownloadService
- Nuget .NET CLI: dotnet add package Vasconcellos.FipeTable.DownloadService

##### Library with the entities for the use of vehicle data from the FIPE table.
- Nuget: https://www.nuget.org/packages/Vasconcellos.FipeTable.Types
- Nuget .NET CLI: dotnet add package Vasconcellos.FipeTable.Types

Example of using the FIPE table library.
```csharp
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Infra;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Enums;

public class Example()
{
    private readonly ILogger _logger;
    private readonly IHttpRequestSettings _httpRequestSettings;
    private readonly IHttpRequest _httpRequest;
    private readonly IFipeDownloadService _downloadService;
    private readonly IFipeNormalizedDownloadService _normalizedDownloadService;

    /// <summary>
    /// Builder.
    /// </summary>
    public Example()
    {
        this._logger = new LoggerFactory().CreateLogger("LoggerFIPE");
        this._httpRequestSettings = new HttpRequestSettings();
        this._httpRequest = new HttpRequest(this._logger, this._httpRequestSettings);
        this._downloadService = new FipeDownloadService(this._logger, this._httpRequest);
        this._normalizedDownloadService = new FipeNormalizedDownloadService(this._logger, this._downloadService);
    }

    public NormalizedDownloadResult GetExample(FipeVehicleTypesEnum vehicleType, int referenceCode = 0)
    {
        var result = this._normalizedDownloadService.GetDataFromFipeTableByVehicleType(vehicleType, referenceCode);
        if(result.VehicleType == vehicleType && result.ReferenceCode == referenceCode
            && result.Brands.Count > 0 && result.Models.Count > 0 && result.Vehicles.Count > 0)
            return result;
        else 
            return null;
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