using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Infra;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.Types.Enums;
using Xunit;

namespace Vasconcellos.FipeTable.UnitTest
{
    public class FipeNormalizedDownloadServiceTest
    {
        private readonly ILogger _logger;
        private readonly IHttpRequestSettings _httpRequestSettings;
        private readonly IHttpRequest _httpRequest;
        private readonly IFipeDownloadService _downloadService;
        private readonly IFipeNormalizedDownloadService _normalizedDownloadService;

        public FipeNormalizedDownloadServiceTest()
        {
            this._logger = new LoggerFactory().CreateLogger(nameof(FipeDownloadServiceTest));
            this._httpRequestSettings = new HttpRequestSettings();
            this._httpRequest = new HttpRequest(this._logger, this._httpRequestSettings);
            this._downloadService = new FipeDownloadService(this._logger, this._httpRequest);
            this._normalizedDownloadService = new FipeNormalizedDownloadService(this._logger, this._downloadService);
        }

        /// <summary>
        /// Get data from FIPE table by vehicle type test
        /// Notes:
        /// - If the reference code is parameterized with the value 0, tthe most current reference will be used, that is, the most current data from the fipe table.
        /// - The slowing in the download is caused that of the proxy of the [FIPE WebAPI], which performs momentary locks.
        /// - To avoid making requests while the proxy is still locked, the service will pause the task for a few minutes and after will perform normal.
        /// - The truck download is the fastest among them.
        /// - Downloading the three types of vehicles together [Car, Motorcycle, Truck/MicroBus] takes between 2 or 4 hours.
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <param name="referenceCode"></param>
        [Theory]
        //[InlineData(FipeVehicleTypesEnum.Car, 245)]
        //[InlineData(FipeVehicleTypesEnum.Motorcycle, 245)]
        [InlineData(FipeVehicleTypesEnum.TruckAndMicroBus, 245)]
        public void GetDataFromFipeTableByVehicleTypeTest(FipeVehicleTypesEnum vehicleType, int referenceCode)
        {
            var result = this._normalizedDownloadService.GetDataFromFipeTableByVehicleType(vehicleType, referenceCode);
            var condition = false;
            var message = "The download of normalized FIPE data was not successful. VehicleType=";

            switch (vehicleType)
            {
                case FipeVehicleTypesEnum.Car:
                    condition = 
                        result.Models.Any(x => x.Description.Contains("uno", StringComparison.OrdinalIgnoreCase)) &&
                        result.Vehicles.Count(x => x.VehicleFuelTypeId == VehicleFuelTypesEnum.Ethanol) > 2 &&
                        result.Vehicles.Count(x => x.VehicleFuelTypeId == VehicleFuelTypesEnum.Diesel) > 2 &&
                        result.Vehicles.Count(x => x.VehicleFuelTypeId == VehicleFuelTypesEnum.Gas) > 2 &&
                        result.Vehicles.Count(x => x.VehicleFuelTypeId == VehicleFuelTypesEnum.Flex) > 2;
                    Assert.True(condition, $"{message}{vehicleType};");
                    goto default;

                case FipeVehicleTypesEnum.Motorcycle:
                    condition = 
                        result.Models.Any(x => x.Description.Contains("ninja", StringComparison.OrdinalIgnoreCase));
                    Assert.True(condition, $"{message}{vehicleType};");
                    goto default;

                case FipeVehicleTypesEnum.TruckAndMicroBus:
                    condition = 
                        result.Models.Any(x => x.Description.Contains("constellation", StringComparison.OrdinalIgnoreCase));
                    Assert.True(condition, $"{message}{vehicleType};");
                    goto default;

                default:
                    Assert.True(
                        result.VehicleType == vehicleType
                        && result.ReferenceCode == referenceCode
                        && result.Brands.Count > 0
                        && result.Brands.Any(x => x.Type == vehicleType)
                        && result.Models.Count > 0
                        && result.Vehicles.Count > 0
                        , $"The download of normalized FIPE data was not successful. VehicleType={vehicleType};");
                    break;
            }
        }
    }
}
