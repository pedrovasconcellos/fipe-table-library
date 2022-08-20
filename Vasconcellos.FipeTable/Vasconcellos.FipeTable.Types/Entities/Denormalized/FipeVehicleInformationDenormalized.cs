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
            IEnumerable<FipeVehicleType> types,
            IEnumerable<FipeVehicleBrand> brands,
            IEnumerable<FipeVehicleModel> models,
            IEnumerable<VehicleFuelType> fuelTypes,
            IEnumerable<FipeVehicleFuelType> fipeFuelTypes)
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

            var model = models
                .FirstOrDefault(model => model.Id == vehicle.FipeVehicleModelId);

            this.FipeVehicleModelDescription = model.Description;

            var brand = brands
                .FirstOrDefault(brand => brand.Id == model.BrandId);

            this.FipeVehicleBrandId = model.Id;
            this.FipeVehicleBrandDescription = model.Description;

            var type = types
                .FirstOrDefault(type => type.Id == brand.VehicleTypeId);

            this.FipeVehicleTypeId = type.Id;
            this.FipeVehicleTypeDescription = type.Description;

            this.VehicleFuelTypeDescription = fuelTypes
                .FirstOrDefault(fuel => fuel.Id == vehicle.VehicleFuelTypeId)
                ?.Description;

            this.FipeVehicleFuelTypeDescription = fipeFuelTypes
                .FirstOrDefault(fuel => fuel.Id == vehicle.FipeVehicleFuelTypeId)
                ?.Description;
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

