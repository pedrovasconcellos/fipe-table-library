using Xunit;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.Types.Enums;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Infra;

namespace Vasconcellos.FipeTable.UnitTest
{
    public class FipeDownloadServiceTest
    {
        private readonly ILogger _logger;
        private readonly IHttpRequestSettings _httpRequestSettings;
        private readonly IHttpRequest _httpRequest;
        private readonly IFipeDownloadService _downloadService;

        public FipeDownloadServiceTest()
        {
            this._logger = new LoggerFactory().CreateLogger(nameof(FipeDownloadServiceTest));
            this._httpRequestSettings = new HttpRequestSettings();
            this._httpRequest = new HttpRequest(this._logger, this._httpRequestSettings);
            this._downloadService = new FipeDownloadService(this._logger, this._httpRequest);
        }

        [Fact]
        public void DownloadListReferenceIdFipeTableTest()
        {
            var referencesCodes = this._downloadService.GetListReferenceIdFipeTable();
            Assert.True(referencesCodes != null && referencesCodes.Count > 0, "Error while downloading list Fipe reference id!");
        }

        [Fact]
        public void DownloadFipeRefenrenceIdTest()
        {
            var fipeReference = this._downloadService.GetFipeTableReference(245);
            Assert.True(fipeReference.Id == 245, "Error while downloading Fipe reference id!");
        }

        [Fact]
        public void DownloadFipeVehicleBrandsTest()
        {
            var fipeReference = this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);
            this._downloadService.GetBrands(fipeTable);
            Assert.True(fipeTable.Brands.Count > 0, "Error downloading vehicle brands!");
        }

        [Fact]
        public void DownloadFipeVehicleModelsTest()
        {
            var fipeReference = this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            this._downloadService.GetModels(fipeTable);
            Assert.True(fipeTable.Brands[0].Models.Count > 0, "Error downloading vehicle models!");
        }

        [Fact]
        public void DownloadFipeVehicleYearAndFuelTest()
        {
            var fipeReference = this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            fipeTable.Brands[0].Models.Add(new Model()
            {
                Value = "1"
            });

            this._downloadService.GetYearsAndFuels(fipeTable);
            Assert.True(fipeTable.Brands[0].Models[0].YearAndFuels.Count > 0, "Error downloading vehicle year and fuel!");
        }

        [Fact]
        public void DownloadFipeVehicleTest()
        {
            var fipeReference = this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            fipeTable.Brands[0].Models.Add(new Model()
            {
                Value = "1"
            });

            fipeTable.Brands[0].Models[0].YearAndFuels.Add(new YearAndFuel()
            {
                Value = "1991-1"
            });

            var vehicles = this._downloadService.GetVehicles(fipeTable);
            Assert.True(vehicles.Count > 0, "Error downloading vehicle!");
        }
    }
}