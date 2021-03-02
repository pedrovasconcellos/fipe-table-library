using System;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleInformation
    {
        public FipeVehicleInformation() 
        { 
        }

        public FipeVehicleInformation(
            string fipeCode,
            long modelId,
            short year,
            VehicleFuelTypesEnum vehicleFuelTypeId,
            string authentication,
            FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId
            )
        {
            var vehicleIsValid = this.VehicleIsValid(
                fipeCode,
                modelId, year, 
                vehicleFuelTypeId, 
                fipeVehicleFuelTypeId);

            var id = this.CreteId(modelId, year, fipeVehicleFuelTypeId);

            if (vehicleIsValid)
            {
                this.Id = id;
                this.Active = true;
                this.IsValid = true;
            }
            else
            {
                this.Id = $"invalid-{Guid.NewGuid()}-{id}";
                this.Active = false;
                this.IsValid = false;
            }
            
            this.FipeCode = fipeCode;
            this.FipeVehicleModelId = modelId;
            this.Year = year;
            this.VehicleFuelTypeId = vehicleFuelTypeId;
            this.Authentication = authentication;
            this.FipeVehicleFuelTypeId = fipeVehicleFuelTypeId;
            this.Created = DateTime.UtcNow;
            this.Updated = null;
        }

        public string Id { get; set; }
        public string FipeCode { get; set; }
        public long FipeVehicleModelId { get; set; }
        public short Year { get; set; }
        public VehicleFuelTypesEnum VehicleFuelTypeId { get; set; }  
        public string Authentication { get; set; }
        public FipeVehicleFuelTypesEnum FipeVehicleFuelTypeId { get; set; }
        public bool IsValid { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Active { get; set; }

        public string CreteId(
            long modelId,
            short year,
            FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId)
        {
            return $"{modelId}-{year}-{(short)fipeVehicleFuelTypeId}";
        }

        private bool VehicleIsValid( 
            string fipeCode, 
            long modelId, 
            short year, 
            VehicleFuelTypesEnum vehicleFuelTypeId,
            FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId)
        {
            var vehicleIsInvalid =
                modelId < 1 ||
                year < 1886 ||
                (year > (DateTime.Now.Year + 1) && year != 32000) ||
                string.IsNullOrEmpty(fipeCode) ||
                !Enum.IsDefined(typeof(VehicleFuelTypesEnum), vehicleFuelTypeId) ||
                !Enum.IsDefined(typeof(FipeVehicleFuelTypesEnum), fipeVehicleFuelTypeId);

            return !vehicleIsInvalid;
        }

        public bool VehicleIsValid() 
        {
            return this.VehicleIsValid(
                this.FipeCode, 
                this.FipeVehicleModelId, 
                this.Year, 
                this.VehicleFuelTypeId,  
                this.FipeVehicleFuelTypeId);
        }
    }
}