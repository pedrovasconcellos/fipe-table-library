using System;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeVehicleInformation
    {
        public Guid Id { get; set; }
        public string FipeCode { get; set; }
        public long FipeVehicleModelId { get; set; }
        public short Year { get; set; }
        public FipeVehicleFuelTypesEnum FipeVehicleFuelTypeId { get; set; }
        public VehicleFuelTypesEnum VehicleFuelTypeId { get; set; }
        public decimal Value { get; set; }      
        public int FipeReferenceId { get; set; }
        public string Authentication { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool Active { get; set; }
    }
}