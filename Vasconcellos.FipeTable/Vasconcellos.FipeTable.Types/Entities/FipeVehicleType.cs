using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleType
    {
        public FipeVehicleType() { }
        public FipeVehicleType(FipeVehicleTypesEnum id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        public FipeVehicleTypesEnum Id { get; set; }
        public string Description { get; set; }
    }
}