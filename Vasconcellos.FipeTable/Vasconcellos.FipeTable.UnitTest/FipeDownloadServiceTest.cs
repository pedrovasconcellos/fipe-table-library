using Xunit;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.Types.Enums;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Infra;
using System.Threading.Tasks;
using System.Linq;

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
        public async Task DownloadListReferenceIdFipeTableTest()
        {
            var referencesCodes = await this._downloadService.GetListReferenceIdFipeTable();
            Assert.True(referencesCodes != null && referencesCodes.Any(), "Error while downloading list Fipe reference id!");
        }

        [Fact]
        public async Task DownloadFipeRefenrenceIdTest()
        {
            var fipeReference = await this._downloadService.GetFipeTableReference(245);
            Assert.True(fipeReference.Id == 245, "Error while downloading Fipe reference id!");
        }

        [Fact]
        public async Task DownloadFipeVehicleBrandsTest()
        {
            var fipeReference = await this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);
            await this._downloadService.GetBrands(fipeTable);
            Assert.True(fipeTable.Brands.Any(), "Error downloading vehicle brands!");
        }

        [Fact]
        public async Task DownloadFipeVehicleModelsTest()
        {
            var fipeReference = await this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            await this._downloadService.GetModels(fipeTable);
            Assert.True(fipeTable.Brands[0].Models.Any(), "Error downloading vehicle models!");
        }

        [Fact]
        public async Task DownloadFipeVehicleYearAndFuelTest()
        {
            var fipeReference = await this._downloadService.GetFipeTableReference();
            var fipeTable = new FipeDataTable(fipeReference.Id, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            fipeTable.Brands[0].Models.Add(new Model()
            {
                Value = "1"
            });

            await this._downloadService.GetYearsAndFuels(fipeTable);
            Assert.True(fipeTable.Brands[0].Models[0].YearAndFuels.Any(), "Error downloading vehicle year and fuel!");
        }

        [Fact]
        public async Task DownloadFipeVehicleTest()
        {
            var fipeReference = await this._downloadService.GetFipeTableReference();
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

            var vehicles = await this._downloadService.GetVehicles(fipeTable);
            Assert.True(vehicles.Any(), "Error downloading vehicle!");
        }
    }
}