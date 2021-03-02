using System;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehiclePrice
    {
        public FipeVehiclePrice() { }

        public FipeVehiclePrice(int fipeReferenceId, decimal value, FipeVehicleInformation vehicleInformation)
        {
            var vehicleInformationId = vehicleInformation?.Id ?? string.Empty;
            this.Builder(fipeReferenceId, vehicleInformationId, value);
        }

        public FipeVehiclePrice(int referenceId, string vehicleInformationId, decimal value)
        {
            this.Builder(referenceId, vehicleInformationId, value);
        }

        public string Id { get; set; }
        public int FipeReferenceId { get; set; }
        public string FipeVehicleInformationId { get; set; }
        public decimal Value { get; set; }
        public bool Active { get; set; }

        private void Builder(int fipeReferenceId, string vehicleInformationId, decimal value)
        {
            var id = this.CreateId(fipeReferenceId, vehicleInformationId);

            var fipeVehiclePriceIsValid = this.FipeVehiclePriceIsValid(
                fipeReferenceId,
                vehicleInformationId,
                value);

            if (fipeVehiclePriceIsValid)
            {
                this.Id = id;
                this.Active = true;
            }
            else
            {
                this.Id = $"invalid-{Guid.NewGuid()}-{id}";
                this.Active = false;
            }

            this.FipeReferenceId = fipeReferenceId;
            this.FipeVehicleInformationId = vehicleInformationId;
            this.Value = value;
        }

        public string CreateId(int fipeReferenceId, string vehicleInformationId)
        {
            return $"{fipeReferenceId}-{vehicleInformationId}";
        }

        private bool FipeVehiclePriceIsValid(int fipeReferenceId, string vehicleInformationId, decimal value)
        {
            var fipeVehiclePriceIsInvalid = 
                fipeReferenceId < 1 ||
                string.IsNullOrEmpty(vehicleInformationId) ||
                value <= 0;

                return !fipeVehiclePriceIsInvalid;
        }

        public bool FipeVehiclePriceIsValid()
        {
            return this.FipeVehiclePriceIsValid(
                this.FipeReferenceId,
                this.FipeVehicleInformationId,
                this.Value);
        }
    }
}
