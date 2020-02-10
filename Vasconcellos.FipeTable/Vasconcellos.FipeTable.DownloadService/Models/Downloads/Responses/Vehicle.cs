using Vasconcellos.FipeTable.Types.Enums;
using System;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class Vehicle
    {
        public Vehicle()
        {
            this.Created = DateTime.Now;
        }

        public string CodigoFipe { get; set; }
        /// <summary>
        /// Vehicle value in R$ updated according to the reference month.
        /// </summary>
        public string Valor { get; set; }
        /// <summary>
        /// Description of the brand name.
        /// </summary>
        public string Marca { get; set; }
        /// <summary>
        /// Description of the vehicle model name.
        /// </summary>
        public string Modelo { get; set; }
        /// <summary>
        /// Vehicle model year.
        /// </summary>
        public short AnoModelo { get; set; }
        /// <summary>
        /// Description of the vehicle's fuel name.
        /// </summary>
        public string Combustivel { get; set; }
        /// <summary>
        /// Reference month of vehicle data in relation to FIPE Table.
        /// </summary>
        public string MesReferencia { get; set; }
        /// <summary>
        /// Fipe authentication code (note: I still don't know what this item is for).
        /// </summary>
        public string Autenticacao { get; set; }
        /// <summary>
        /// Vehicle type number (Example: Car, Motorcycle or Truck).
        /// </summary>
        public FipeVehicleTypesEnum TipoVeiculo { get; set; }
        /// <summary>
        /// First letter of fuel.
        /// </summary>
        public string SiglaCombustivel { get; set; }
        /// <summary>
        /// Date and time the vehicle information was consulted
        /// </summary>
        public string DataConsulta { get; set; }

        #region NotUsedInDeserialization
        internal int ReferenceID { get; set; }
        internal string BrandID { get; set; }
        internal string ModelID { get; set; }
        internal FipeVehicleFuelTypesEnum FuelID { get; set; }
        internal short Year { get; set; }
        internal DateTime Created { get; private set; }
        internal double Value => Convert.ToDouble(this.Valor?.Substring(2)?.Replace(".", "").Replace(",", "."));
        #endregion NotUsedInDeserialization
    }
}
