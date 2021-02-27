using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class VehicleFuelType
    {
        public VehicleFuelType() { }
        public VehicleFuelType(VehicleFuelTypesEnum id, string description)
        {
            this.Id = id;
            this.Description = description;
            this.Active = true;
        }

        public VehicleFuelTypesEnum Id { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
    }
}