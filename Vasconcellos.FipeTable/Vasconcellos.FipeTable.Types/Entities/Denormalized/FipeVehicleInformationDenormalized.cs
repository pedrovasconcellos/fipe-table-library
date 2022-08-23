using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.Types.Entities.Denormalized
{
    public class FipeVehicleInformationDenormalized : FipeVehicleInformation
    {
        public FipeVehicleInformationDenormalized(FipeVehicleInformation vehicle,
            FipeVehicleTypesEnum vehicleTypeId,
            long brandId,
            string brandDescription,
            string modelDescription,
            VehicleFuelTypesEnum vehicluelTypeId,
            FipeVehicleFuelTypesEnum fipeVehicleFuelTypeId)
        {
            base.Id = vehicle.Id;
            base.FipeCode = vehicle.FipeCode;
            base.FipeVehicleModelId = vehicle.FipeVehicleModelId;
            base.Year = vehicle.Year;
            base.VehicleFuelTypeId = vehicle.VehicleFuelTypeId;
            base.Authentication = vehicle.Authentication;
            base.FipeVehicleFuelTypeId = vehicle.FipeVehicleFuelTypeId;
            base.IsValid = vehicle.IsValid;
            base.Created = vehicle.Created;
            base.Updated = vehicle.Updated;
            base.Active = vehicle.Active;

            this.FipeVehicleModelDescription = modelDescription;
            this.FipeVehicleBrandId = brandId;
            this.FipeVehicleBrandDescription = brandDescription;
            this.FipeVehicleTypeId = vehicleTypeId;
            this.FipeVehicleTypeDescription = vehicleTypeId.GetDescription();
            this.VehicleFuelTypeDescription = vehicluelTypeId.GetDescription();
            this.FipeVehicleFuelTypeDescription = fipeVehicleFuelTypeId.GetDescription();
        }

        public FipeVehicleTypesEnum FipeVehicleTypeId { get; set; }

        public string FipeVehicleTypeDescription { get; set; }

        public long FipeVehicleBrandId { get; set; }

        public string FipeVehicleBrandDescription { get; set; }

        public string FipeVehicleModelDescription { get; set; }

        public string VehicleFuelTypeDescription { get; set; }

        public string FipeVehicleFuelTypeDescription { get; set; }
    }
}

