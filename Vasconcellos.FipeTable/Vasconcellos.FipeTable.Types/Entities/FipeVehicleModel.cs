
namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleModel
    {
        public FipeVehicleModel() { }
        public FipeVehicleModel(long id, string description, long brandId)
        {
            this.Id = id;
            this.Description = description;
            this.BrandId = brandId;
            this.Active = true;
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public long BrandId { get; set; }
        public bool Active { get; set; }
    }
}