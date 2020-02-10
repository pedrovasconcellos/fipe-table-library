using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long BrandId { get; set; }
        public bool Active { get; set; }
    }
}