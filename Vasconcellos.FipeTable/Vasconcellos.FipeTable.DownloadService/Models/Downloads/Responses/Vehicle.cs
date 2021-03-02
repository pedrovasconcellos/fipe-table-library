using Vasconcellos.FipeTable.Types.Enums;
using System;
using System.Linq;

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
        /// Date and time the vehicle information was consulted.
        /// </summary>
        public string DataConsulta { get; set; }

        #region NotUsedInDeserialization
        public int ReferenceId { get; set; }
        public string BrandId { get; set; }
        public string ModelId { get; set; }
        public FipeVehicleFuelTypesEnum FipeVehicleFuelTypeId { get; set; }
        public VehicleFuelTypesEnum VehicleFuelTypeId { get => GetVehicleFuelTypeEnum(); }
        public short Year { get; set; }
        public DateTime Created { get; private set; }
        public decimal Value { get => Convert.ToDecimal(this.Valor?.Substring(2)?.Replace(".", "").Replace(",", ".")); }
        #endregion NotUsedInDeserialization

        internal void SetAdditionalInformation(int referenceId, Brand brand, Model model, YearAndFuel yearAndFuel)
        {
            this.ReferenceId = referenceId;
            this.BrandId = brand.Value;
            this.ModelId = model.Value;
            this.Year = yearAndFuel.Year;
            this.FipeVehicleFuelTypeId = yearAndFuel.Fuel;
        }

        #region VehicleFuelTypeDefinition
        private static readonly string[] _flex = { "flex ", " flex", "flexpower", ".flex", "/flex", "econoflex", "blueflex", "-flex" };
        private static readonly string[] _gas = { "gas.", "gas " };
        private static readonly string[] _eletric = { "(eletric" };

        private VehicleFuelTypesEnum GetVehicleFuelTypeEnum()
        {
            if (!Enum.IsDefined(typeof(FipeVehicleFuelTypesEnum), this.FipeVehicleFuelTypeId)
                || string.IsNullOrEmpty(this.Modelo))
                return 0;

            var modelDescription = this.Modelo.ToLower();

            if (_flex.Any(x => modelDescription.Contains(x)))
                return VehicleFuelTypesEnum.Flex;

            if (_gas.Any(x => modelDescription.Contains(x)))
                return VehicleFuelTypesEnum.Gas;

            if (_eletric.Any(x => modelDescription.Contains(x)))
                return VehicleFuelTypesEnum.Electric;

            return (VehicleFuelTypesEnum)this.FipeVehicleFuelTypeId;
        }
        #endregion VehicleFuelTypeDefinition
    }
}
