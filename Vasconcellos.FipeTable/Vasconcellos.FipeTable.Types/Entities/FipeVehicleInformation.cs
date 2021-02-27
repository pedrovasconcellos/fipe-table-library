using System;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleInformation
    {
        public FipeVehicleInformation() { }
        public FipeVehicleInformation(
            long brandId,
            string fipeCode,
            int fipeReferenceCode,
            long modelId, 
            short year,
            VehicleFuelTypesEnum vehicleFuelTypeId,
            decimal value,
            string authentication,
            FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId
            )
        {
            if (brandId < 1 || 
                string.IsNullOrEmpty(fipeCode) ||
                fipeReferenceCode < 0 || 
                modelId < 1 || 
                year < 1886 || 
                year > (DateTime.Now.Year + 1) || 
                !Enum.IsDefined(typeof(VehicleFuelTypesEnum), vehicleFuelTypeId) ||
                value <= 0  ||
                !Enum.IsDefined(typeof(FipeVehicleFuelTypesEnum), fipeVehicleFuelTypeId))
                return;

            this.Id = $"{fipeReferenceCode}-{brandId}-{modelId}-{year}-{(short)vehicleFuelTypeId}";
            this.FipeCode = fipeCode;
            this.FipeReferenceCode = fipeReferenceCode;
            this.FipeVehicleModelId = modelId;
            this.Year = year;
            this.VehicleFuelTypeId = vehicleFuelTypeId;
            this.Value = value;
            this.Authentication = authentication;
            this.FipeVehicleFuelTypeId = fipeVehicleFuelTypeId;
            this.Created = DateTime.UtcNow;
            this.Updated = null;
            this.Active = true;
        }

        public string Id { get; set; }
        public string FipeCode { get; set; }
        public int FipeReferenceCode { get; set; }
        public long FipeVehicleModelId { get; set; }
        public short Year { get; set; }
        public VehicleFuelTypesEnum VehicleFuelTypeId { get; set; }
        public decimal Value { get; set; }      
        public string Authentication { get; set; }
        public FipeVehicleFuelTypesEnum FipeVehicleFuelTypeId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Active { get; set; }
    }
}