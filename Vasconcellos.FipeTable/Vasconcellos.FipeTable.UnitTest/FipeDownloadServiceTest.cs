using Xunit;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Vasconcellos.FipeTable.DownloadService.Services;
using Vasconcellos.FipeTable.Types.Enums;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;

namespace Vasconcellos.FipeTable.UnitTest
{
    public class FipeDownloadServiceTest
    {
        private readonly ILogger _logger;
        private readonly IFipeDownloadService _fipeDownloadService;

        public FipeDownloadServiceTest()
        {
            this._logger = new LoggerFactory().CreateLogger(nameof(FipeDownloadServiceTest));
            this._fipeDownloadService = new FipeDownloadService(this._logger);
        }

        [Fact]
        public void DownloadListReferenceCodeFipeTableTest()
        {
            var referencesCodes = this._fipeDownloadService.GetListReferenceCodeFipeTable();
            Assert.True(referencesCodes != null && referencesCodes.Count > 0, "Error while downloading list Fipe reference code!");
        }

        [Fact]
        public void DownloadFipeRefenrenceIdTest()
        {
            var referenceId = this._fipeDownloadService.GetFipeTableReferenceCode(245);
            Assert.True(referenceId == 245, "Error while downloading Fipe reference code!");
        }

        [Fact]
        public void DownloadFipeVehicleBrandsTest()
        {
            var referenceId = this._fipeDownloadService.GetFipeTableReferenceCode();
            var fipeTable = new FipeDataTable(referenceId, FipeVehicleTypesEnum.Car);
            this._fipeDownloadService.GetBrands(fipeTable);
            Assert.True(fipeTable.Brands.Count > 0, "Error downloading vehicle brands!");
        }

        [Fact]
        public void DownloadFipeVehicleModelsTest()
        {
            var referenceId = this._fipeDownloadService.GetFipeTableReferenceCode();
            var fipeTable = new FipeDataTable(referenceId, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            this._fipeDownloadService.GetModels(fipeTable);
            Assert.True(fipeTable.Brands[0].Models.Count > 0, "Error downloading vehicle models!");
        }

        [Fact]
        public void DownloadFipeVehicleYearAndFuelTest()
        {
            var referenceId = this._fipeDownloadService.GetFipeTableReferenceCode();
            var fipeTable = new FipeDataTable(referenceId, FipeVehicleTypesEnum.Car);

            fipeTable.Brands.Add(new Brand()
            {
                Value = "1"
            });

            fipeTable.Brands[0].Models.Add(new Model()
            {
                Value = "1"
            });

            this._fipeDownloadService.GetYearsAndFuels(fipeTable);
            Assert.True(fipeTable.Brands[0].Models[0].YearAndFuels.Count > 0, "Error downloading vehicle year and fuel!");
        }

        [Fact]
        public void DownloadFipeVehicleTest()
        {
            var referenceId = this._fipeDownloadService.GetFipeTableReferenceCode();
            var fipeTable = new FipeDataTable(referenceId, FipeVehicleTypesEnum.Car);

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

            var vehicles = this._fipeDownloadService.GetVehicles(fipeTable);
            Assert.True(vehicles.Count > 0, "Error downloading vehicle!");
        }
    }
}