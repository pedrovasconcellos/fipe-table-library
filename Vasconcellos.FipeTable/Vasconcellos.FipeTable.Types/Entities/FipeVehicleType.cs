using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleType
    {
        public FipeVehicleType() { }
        public FipeVehicleType(FipeVehicleTypesEnum fipeVehicleTypesEnum)
        {
            this.Id = fipeVehicleTypesEnum;
            this.Description = fipeVehicleTypesEnum.ToString();
            this.Active = true;
        }

        public FipeVehicleTypesEnum Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}