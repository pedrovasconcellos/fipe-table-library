using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleBrand
    {
        public FipeVehicleBrand(long id, string description, FipeVehicleTypesEnum type) 
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
            this.Active = true;
        }

        public long Id { get; private set; }
        public string Description { get; private set; }
        public FipeVehicleTypesEnum Type { get; private set; }
        public bool Active { get; private set; }
    }
}