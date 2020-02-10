using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleBrand
    {
        public long Id { get; set; }
        public FipeVehicleTypesEnum Type { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}