using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleFuelType
    {
        public FipeVehicleFuelType(FipeVehicleFuelTypesEnum id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        public FipeVehicleFuelTypesEnum Id { get; private set; }
        public string Description { get; private set; }
    }
}