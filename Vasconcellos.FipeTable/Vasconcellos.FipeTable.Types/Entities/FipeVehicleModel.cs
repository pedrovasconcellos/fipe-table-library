
namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleModel
    {
        public FipeVehicleModel(long id, string description, long brandId)
        {
            this.Id = id;
            this.Description = description;
            this.BrandId = brandId;
            this.Active = true;
        }

        public long Id { get; private set; }
        public string Description { get; private set; }
        public long BrandId { get; private set; }
        public bool Active { get; private set; }
    }
}