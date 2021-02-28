using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleBrand
    {
        public FipeVehicleBrand() { }
        public FipeVehicleBrand(long id, string description, FipeVehicleTypesEnum fipeVehicleTypesEnum) 
        {
            this.Id = id;
            this.Description = description;
            this.VehicleTypeId = fipeVehicleTypesEnum;
            this.Active = true;
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public FipeVehicleTypesEnum VehicleTypeId { get; set; }
        public bool Active { get; set; }
    }
}