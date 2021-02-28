using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleFuelType
    {
        public FipeVehicleFuelType() { }
        public FipeVehicleFuelType(FipeVehicleFuelTypesEnum fipeVehicleFuelTypesEnum)
        {
            this.Id = fipeVehicleFuelTypesEnum;
            this.Description = fipeVehicleFuelTypesEnum.ToString();
            this.Active = true;
        }

        public FipeVehicleFuelTypesEnum Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}