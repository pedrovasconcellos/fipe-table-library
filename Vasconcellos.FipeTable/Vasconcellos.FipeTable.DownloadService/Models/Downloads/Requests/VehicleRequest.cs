using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Models.Requests
{
    public static class VehicleRequest
    {
        public static dynamic Brand(FipeDataTable fipeTable)
        {
            return new
            {
                CodigoTabelaReferencia = fipeTable.ReferenceCode,
                CodigoTipoVeiculo = fipeTable.VehicleType
            };
        }

        public static dynamic Model(FipeDataTable fipeTable, string brandCode)
        {
            return new
            {
                CodigoTabelaReferencia = fipeTable.ReferenceCode,
                CodigoTipoVeiculo = fipeTable.VehicleType,
                CodigoMarca = brandCode,
            };
        }

        public static dynamic YearAndFuel(FipeDataTable fipeTable, string brandCode, string modelCode)
        {
            return new
            {
                CodigoTabelaReferencia = fipeTable.ReferenceCode,
                CodigoTipoVeiculo = fipeTable.VehicleType,
                codigoMarca = brandCode,
                codigoModelo = modelCode,
            };
        }

        public static dynamic Vehicle(FipeDataTable fipeTable, string brandCode, string modelCode, int year, FipeVehicleFuelTypesEnum fuelType)
        {
            return new
            {
                CodigoTabelaReferencia = fipeTable.ReferenceCode,
                CodigoTipoVeiculo = fipeTable.VehicleType,
                codigoMarca = brandCode,
                codigoModelo = modelCode,
                anoModelo = year,
                codigoTipoCombustivel = fuelType,
                tipoConsulta = "tradicional"
            };
        }
    }
}
