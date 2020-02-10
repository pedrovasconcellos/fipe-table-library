# Fipe Table Library [en-us]

### Biblioteca da tabela Fipe [pt-br]
### Description: Library for using vehicles data from the Fipe Table.

#### License: MIT License
#### Copyright (c) 2020 Pedro Vasconcellos
##### Author: Pedro Henrique Vasconcellos
##### Site: https://vasconcellos.solutions

Usege tips.
- Use the [FipeVehicleTypesEnum] enumerator to say what type of vehicle you want to search for.
- Types of vehicles contained in the enumerator [FipeVehicleTypesEnum] (Car = 1, Motorcycle = 2, TruckAndMicroBus = 3)
- If the (referenceCode == 0), the most current reference will be used, that is, the most current data from the fipe table.
- Warning: Do not call more than one FIPE library download service method in parallel, as the Proxy FIPE will block your IP for a certain time (5-10 minutes maybe).
- If you want to create the tables in the relational database [MSSQL / SqlServer], there are SQL scripts in the directory=[/Vasconcellos.FipeTable.Database/Tables/].
- if you see [Vehicle.Year=32000] want say that the vehicle is zero KM.

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

    public GetExample(FipeVehicleTypesEnum vehicleType, int referenceCode = 0)
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

Method [GetExample] return object.
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