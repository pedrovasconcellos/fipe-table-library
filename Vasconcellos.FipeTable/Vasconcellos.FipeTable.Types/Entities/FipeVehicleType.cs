using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleType
    {
        public FipeVehicleType(FipeVehicleTypesEnum id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        public FipeVehicleTypesEnum Id { get; private set; }
        public string Description { get; private set; }
    }
}