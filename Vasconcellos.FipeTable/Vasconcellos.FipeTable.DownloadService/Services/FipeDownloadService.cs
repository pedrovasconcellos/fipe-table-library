﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Vasconcellos.FipeTable.Types.Exceptions;
using Vasconcellos.FipeTable.DownloadService.Models.Requests;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.DownloadService.Services.Interfaces;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;

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
        /// Downloads the list fipe table monthly reference code.
        /// </summary>
        /// <returns>List<Reference></returns>
        /// <returns>FipeNotFoundException</returns>
        public List<Reference> GetListReferenceCodeFipeTable()
        {
            var referenceTable = this._http.Post<List<Reference>>("ConsultarTabelaDeReferencia", null);
            if (referenceTable == null || referenceTable.Count == 0)
                throw new FipeNotFoundException($"The FIPE table reference list not found.");

            return referenceTable;
        }

        /// <summary>
        /// Downloads the fipe table monthly reference code.
        /// Note: If the reference code is parameterized with the value 0, the most current reference will be used, that is, the most current data from the fipe table.
        /// </summary>
        /// <param name="requestReferenceCode"></param>
        /// <returns>int</returns>
        /// <returns>FipeNotFoundException</returns>
        /// <returns>ArgumentException [if request return == 0]</returns>
        public int GetFipeTableReferenceCode(int requestReferenceCode = 0)
        {
            var referenceTable = this.GetListReferenceCodeFipeTable();
            int referenceCode;

            if (requestReferenceCode == 0)
                referenceCode = referenceTable.Select(x => x.Codigo).OrderByDescending(x => x).FirstOrDefault();
            else
                referenceCode = referenceTable.Select(x => x.Codigo).FirstOrDefault(x => x == requestReferenceCode);

            if (referenceCode == 0)
                throw new FipeArgumentException("Invalid reference code.");

            return referenceCode;
        }

        /// <summary>
        /// Downloads the vehicle brands from the fipe table, by reference code and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>void</returns>
        /// <returns>FipeNotFoundException</returns>
        public void GetBrands(FipeDataTable fipeTable)
        {
            var objRequest = VehicleRequest.Brand(fipeTable);

            List<Brand> list = this._http.Post<List<Brand>>("ConsultarMarcas", objRequest);
            if (list == null || list.Count == 0)
                throw new FipeNotFoundException($"The Brand table not found.");

            fipeTable.Brands = list;
        }

        /// <summary>
        /// Downloads the vehicle models from the fipe table, by reference code and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>void</returns>
        /// <returns>FipeNotFoundException</returns>
        public void GetModels(FipeDataTable fipeTable)
        {
            foreach (var brand in fipeTable.Brands)
            {
                var objRequest = VehicleRequest.Model(fipeTable, brand.Value);

                //Note: The AuxModel class was used to standardize the return;
                AuxModel auxModel = this._http.Post<AuxModel>("ConsultarModelos", objRequest);
                if(auxModel == null || auxModel.Modelos.Count == 0)
                    throw new FipeNotFoundException($"The Model Brand table not found.");

                brand.Models = auxModel.Modelos;
            }
        }

        /// <summary>
        /// Downloads the vehicle years and fuels from the fipe table, by reference code and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>void</returns>
        /// <returns>FipeNotFoundException</returns>
        public void GetYearsAndFuels(FipeDataTable fipeTable)
        {
            foreach (var brand in fipeTable.Brands)
            {
                foreach (var model in brand.Models)
                {
                    var objRequest = VehicleRequest.YearAndFuel(fipeTable, brand.Value, model.Value);

                    List<YearAndFuel> list = this._http.Post<List<YearAndFuel>>("ConsultarAnoModelo", objRequest);
                    if (list == null || list.Count == 0)
                        throw new FipeNotFoundException($"The Year and Fuel table not found.");

                    model.YearAndFuels = list;
                }
            }
        }

        /// <summary>
        /// Downloads the vehicle information from the fipe table, by reference code and vehicle type.
        /// </summary>
        /// <param name="fipeTable"></param>
        /// <returns>List<Vehicle></returns>
        /// <returns>FipeNotFoundException</returns>
        public List<Vehicle> GetVehicles(FipeDataTable fipeTable)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (var brand in fipeTable.Brands)
            {
                foreach (var model in brand.Models)
                {
                    foreach (var yearAndFuel in model.YearAndFuels)
                    {
                        var objRequest = VehicleRequest.Vehicle(fipeTable, brand.Value, model.Value, yearAndFuel.Year, yearAndFuel.Fuel);

                        Vehicle vehicle = this._http.Post<Vehicle>("ConsultarValorComTodosParametros", objRequest);
                        if (vehicle == null || string.IsNullOrEmpty(vehicle.CodigoFipe))
                        {
                            _logger.LogWarning("Vehicle not found.", 
                                fipeTable.ReferenceCode, 
                                brand.Value, model.Value, 
                                yearAndFuel.Year, 
                                yearAndFuel.Fuel);
                        }
                        else
                        {
                            vehicle.SetAdditionalInformation(fipeTable.ReferenceCode, brand, model, yearAndFuel);
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }

            if(vehicles == null || vehicles.Count == 0)
                throw new FipeNotFoundException($"The Vehicle table not found.");

            return vehicles;
        }
    }
}
