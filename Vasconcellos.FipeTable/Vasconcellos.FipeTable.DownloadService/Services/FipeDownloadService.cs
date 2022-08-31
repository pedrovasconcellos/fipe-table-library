using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Vasconcellos.FipeTable.Types.Exceptions;
using Vasconcellos.FipeTable.DownloadService.Models.Requests;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using Vasconcellos.FipeTable.Types.Entities;
using System.Threading.Tasks;

namespace Vasconcellos.FipeTable.DownloadService.Services
{
    /// <summary>
    /// Fipe table download service.
    /// </summary>
    public class FipeDownloadService : IFipeDownloadService
    {
        private readonly ILogger _logger;
        private readonly IHttpRequest _http;

        /// <summary>
        /// FipeDownloadService builder.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpRequest"></param>
        public FipeDownloadService(ILogger logger, IHttpRequest httpRequest)
        {
            this._logger = logger;
            this._http = httpRequest;
        }

        /// <summary>
        /// Downloads the list fipe table monthly reference id.
        /// </summary>
        /// <returns>List<Reference></returns>
        /// <returns>FipeNotFoundException</returns>
        public async Task<List<Reference>> GetListReferenceIdFipeTable()
        {
            var referenceTable = await this._http.Post<List<Reference>>("ConsultarTabelaDeReferencia", null);
            if (referenceTable == null || !referenceTable.Any())
                throw new FipeNotFoundException($"The FIPE table reference list not found.");

            return referenceTable;
        }

        /// <summary>
        /// Downloads the fipe table monthly reference id.
        /// Note: If the reference id is parameterized with the value 0, the most current reference will be used, that is, the most current data from the fipe table.
        /// </summary>
        /// <param name="requestReferenceId"></param>
        /// <returns>int</returns>
        /// <returns>FipeNotFoundException</returns>
        /// <returns>ArgumentException [if request return == 0]</returns>
        public async Task<FipeReference> GetFipeTableReference(int requestReferenceId = 0)
        {
            var referenceTable = await this.GetListReferenceIdFipeTable();
            FipeReference fipeReference;

            if (requestReferenceId == 0)
                fipeReference = referenceTable
                    .Select(x => new FipeReference(x.Codigo, x.Mes, x.ReferenceDate))
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
            else
                fipeReference = referenceTable
                    .Select(x => new FipeReference(x.Codigo, x.Mes, x.ReferenceDate))
                    .SingleOrDefault(x => x.Id == requestReferenceId);

            if (fipeReference == null || fipeReference.Id == 0)
                throw new FipeArgumentException("Invalid FipeReference.");

            return fipeReference;
        }

        /// <summary>
        /// Downloads the vehicle brands from the fipe table, by reference id and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>void</returns>
        /// <returns>FipeNotFoundException</returns>
        public async Task GetBrands(FipeDataTable fipeTable)
        {
            var objRequest = VehicleRequest.Brand(fipeTable);

            List<Brand> list = await this._http.Post<List<Brand>>("ConsultarMarcas", objRequest);
            if (list == null || !list.Any())
                throw new FipeNotFoundException($"The Brand table not found.");

            fipeTable.Brands = list;
        }

        /// <summary>
        /// Downloads the vehicle models from the fipe table, by reference id and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>void</returns>
        /// <returns>FipeNotFoundException</returns>
        public async Task GetModels(FipeDataTable fipeTable)
        {
            foreach (var brand in fipeTable.Brands)
            {
                var objRequest = VehicleRequest.Model(fipeTable, brand.Value);

                //Note: The AuxModel class was used to standardize the return;
                AuxModel auxModel = await this._http.Post<AuxModel>("ConsultarModelos", objRequest);
                if (auxModel == null || !auxModel.Modelos.Any())
                    _logger.LogWarning(
                        $"The Model Brand table not found. ReferenceId={fipeTable.ReferenceId};BrandId={brand.Value};",
                        fipeTable.ReferenceId,
                        brand.Value);
                else
                    brand.Models = auxModel.Modelos;
            }
        }

        /// <summary>
        /// Downloads the vehicle years and fuels from the fipe table, by reference id and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>void</returns>
        /// <returns>FipeNotFoundException</returns>
        public async Task GetYearsAndFuels(FipeDataTable fipeTable)
        {
            foreach (var brand in fipeTable.Brands)
            {
                foreach (var model in brand.Models)
                {
                    var objRequest = VehicleRequest.YearAndFuel(fipeTable, brand.Value, model.Value);

                    List<YearAndFuel> list = await this._http.Post<List<YearAndFuel>>("ConsultarAnoModelo", objRequest);
                    if (list == null || !list.Any())
                        _logger.LogWarning(
                            $"The Year and Fuel table not found. ReferenceId={fipeTable.ReferenceId};BrandId={brand.Value};ModelId={model.Value};",
                            fipeTable.ReferenceId,
                            brand.Value,
                            model.Value);
                    else
                        model.YearAndFuels = list;
                }
            }
        }

        /// <summary>
        /// Downloads the vehicle information from the fipe table, by reference id and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>List<Vehicle></returns>
        /// <returns>FipeNotFoundException</returns>
        public async Task<List<Vehicle>> GetVehicles(FipeDataTable fipeTable)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (var brand in fipeTable.Brands)
            {
                foreach (var model in brand.Models)
                {
                    foreach (var yearAndFuel in model.YearAndFuels)
                    {
                        var objRequest = VehicleRequest.Vehicle(fipeTable, brand.Value, model.Value, yearAndFuel.Year, yearAndFuel.Fuel);

                        Vehicle vehicle = await this._http.Post<Vehicle>("ConsultarValorComTodosParametros", objRequest);
                        if (vehicle == null || string.IsNullOrEmpty(vehicle.CodigoFipe))
                        {
                            _logger.LogWarning(
                                $"Vehicle not found. ReferenceId={fipeTable.ReferenceId};BrandId={brand.Value};ModelId={model.Value};Year={yearAndFuel.Year};FuelId={(short)yearAndFuel.Fuel};",
                                fipeTable.ReferenceId,
                                brand.Value,
                                model.Value,
                                yearAndFuel.Year,
                                yearAndFuel.Fuel);
                        }
                        else
                        {
                            vehicle.SetAdditionalInformation(fipeTable.ReferenceId, brand, model, yearAndFuel);
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }

            if (vehicles == null || !vehicles.Any())
                throw new FipeNotFoundException($"The Vehicle table not found.");

            return vehicles;
        }
    }
}