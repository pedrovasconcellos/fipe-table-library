using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.DownloadService.Profiles;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Entities.Denormalized;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Services
{
    /// <summary>
    /// Fipe table download service with normalize the data [third normal form (3NF)].
    /// </summary>
    public class FipeNormalizedDownloadService : IFipeNormalizedDownloadService
    {
        private readonly ILogger _logger;
        private readonly IFipeDownloadService _downloadService;

        /// <summary>
        /// FipeNormalizedDownloadService builder.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpRequest"></param>
        public FipeNormalizedDownloadService(ILogger logger, IFipeDownloadService downloadService)
        {
            this._logger = logger;
            this._downloadService = downloadService;
        }

        /// <summary>
        /// Get normalized Fipe table data by vehicle type and reference id.
        /// Notes:
        /// - If the reference id is parameterized with the value 0, tthe most current reference will be used, that is, the most current data from the fipe table.
        /// - The slowing in the download is caused that of the proxy of the [FIPE WebAPI], which performs momentary locks.
        /// - To avoid making requests while the proxy is still locked, the service will pause the task for a few minutes and after will perform normal.
        /// - The truck download is the fastest among them.
        /// - Downloading the three types of vehicles together [Car, Motorcycle, Truck/MicroBus] takes between 2 or 4 hours.
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public NormalizedDownloadResult GetDataFromFipeTableByVehicleType(FipeVehicleTypesEnum vehicleType, int referenceId = 0)
        {
            this._logger.LogDebug($"Starting method GetDataFromFipeTableByVehicleType. Method={nameof(this.GetDataFromFipeTableByVehicleType)}; VehicleType={vehicleType}; ReferenceId={referenceId};");

            var selectedReference = this._downloadService.GetFipeTableReference(referenceId);

            var fipeTable = new FipeDataTable(selectedReference.Id, vehicleType);

            this._downloadService.GetBrands(fipeTable);
            this._downloadService.GetModels(fipeTable);
            this._downloadService.GetYearsAndFuels(fipeTable);
            var tupleEntities = fipeTable.ModelToEntity();

            this._logger.LogDebug($"Brands and models of normalized vehicles. Method={nameof(this.GetDataFromFipeTableByVehicleType)}; VehicleType={vehicleType}; ReferenceId={referenceId};");

            var vehicles = this._downloadService.GetVehicles(fipeTable);
            var tuple = vehicles.ModelToEntity();

            this._logger.LogDebug($"Normalized vehicles. Method={nameof(this.GetDataFromFipeTableByVehicleType)}; VehicleType={vehicleType}; ReferenceId={referenceId};");

            return new NormalizedDownloadResult(
                selectedReference, 
                vehicleType, 
                tupleEntities.Brands, 
                tupleEntities.Models,
                tuple.Vehicles,
                tuple.Prices,
                tuple.VehiclesDenormalized);
        }
    }
}
