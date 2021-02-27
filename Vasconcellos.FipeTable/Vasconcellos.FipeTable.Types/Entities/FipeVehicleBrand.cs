using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleBrand
    {
        public FipeVehicleBrand() { }
        public FipeVehicleBrand(long id, string description, FipeVehicleTypesEnum type) 
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
            this.Active = true;
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public FipeVehicleTypesEnum Type { get; set; }
        public bool Active { get; set; }
    }
}