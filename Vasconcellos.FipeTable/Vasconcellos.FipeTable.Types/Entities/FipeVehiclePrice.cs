using System;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehiclePrice
    {
        public FipeVehiclePrice() { }

        public FipeVehiclePrice(int fipeReferenceId, DateTime referenceDate, decimal value, FipeVehicleInformation vehicleInformation)
        {
            var vehicleInformationId = vehicleInformation?.Id ?? string.Empty;
            this.Builder(fipeReferenceId, referenceDate, vehicleInformationId, value);
        }

        public FipeVehiclePrice(int fipeReferenceId, DateTime referenceDate, string vehicleInformationId, decimal value)
        {
            this.Builder(fipeReferenceId, referenceDate, vehicleInformationId, value);
        }

        public string Id { get; set; }
        public int FipeReferenceId { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string FipeVehicleInformationId { get; set; }
        public decimal Value { get; set; }
        public bool Active { get; set; }

        private void Builder(int fipeReferenceId, DateTime referenceDate, string vehicleInformationId, decimal value)
        {
            var id = this.CreateId(fipeReferenceId, vehicleInformationId);

            var fipeVehiclePriceIsValid = this.FipeVehiclePriceIsValid(
                fipeReferenceId,
                referenceDate,
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
            this.ReferenceDate = referenceDate;
            this.FipeVehicleInformationId = vehicleInformationId;
            this.Value = value;
        }

        public string CreateId(int fipeReferenceId, string vehicleInformationId)
        {
            return $"{fipeReferenceId}-{vehicleInformationId}";
        }

        private bool FipeVehiclePriceIsValid(int fipeReferenceId, DateTime referenceDate, string vehicleInformationId, decimal value)
        {
            var fipeVehiclePriceIsInvalid = 
                fipeReferenceId < 1 ||
                referenceDate == default ||
                string.IsNullOrEmpty(vehicleInformationId) ||
                value <= 0;

                return !fipeVehiclePriceIsInvalid;
        }

        public bool FipeVehiclePriceIsValid()
        {
            return this.FipeVehiclePriceIsValid(
                this.FipeReferenceId,
                this.ReferenceDate,
                this.FipeVehicleInformationId,
                this.Value);
        }
    }
}
