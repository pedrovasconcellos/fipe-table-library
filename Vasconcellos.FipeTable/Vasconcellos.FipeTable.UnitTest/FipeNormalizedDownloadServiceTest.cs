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

        [Theory]
        [InlineData(FipeVehicleTypesEnum.TruckAndMicroBus, 245)]
        public void GetDataFromFipeTableByVehicleTypeTest(FipeVehicleTypesEnum vehicleType, int referenceCode)
        {
            var result = this._normalizedDownloadService.GetDataFromFipeTableByVehicleType(vehicleType, referenceCode);
            Assert.True(
                result.VehicleType == vehicleType
                && result.ReferenceCode == referenceCode
                && result.Brands.Count > 0
                && result.Models.Count > 0
                && result.Vehicles.Count > 0, "The download of normalized FIPE data was not successful");
        }
    }
}
