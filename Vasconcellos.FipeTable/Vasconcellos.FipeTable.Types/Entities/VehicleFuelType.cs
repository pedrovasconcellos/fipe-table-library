using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class VehicleFuelType
    {
        public VehicleFuelTypesEnum Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}