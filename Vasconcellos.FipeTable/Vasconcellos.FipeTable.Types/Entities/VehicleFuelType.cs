using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class VehicleFuelType
    {
        public VehicleFuelType() { }
        public VehicleFuelType(VehicleFuelTypesEnum vehicleFuelTypesEnum)
        {
            this.Id = vehicleFuelTypesEnum;
            this.Description = vehicleFuelTypesEnum.ToString();
            this.Active = true;
        }

        public VehicleFuelTypesEnum Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}