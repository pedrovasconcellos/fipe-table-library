using System;
using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Entities;

namespace Vasconcellos.FipeTable.DownloadService.Profiles
{
    public static class FipeVehicleProfile
    {
        public static List<FipeVehicleInformation> ModelToEntity(this IEnumerable<Vehicle> vehicles)
        {
            var vehiclesEntity = new List<FipeVehicleInformation>();

            foreach (var vehicle in vehicles)
            {
                vehiclesEntity.Add(new FipeVehicleInformation()
                {
                    FipeCode = vehicle.CodigoFipe,
                    FipeVehicleModelId = Convert.ToInt64(vehicle.ModelId),
                    Value = vehicle.Value,
                    Year = vehicle.Year,
                    FipeVehicleFuelTypeId = vehicle.FipeVehicleFuelTypeId,
                    VehicleFuelTypeId = vehicle.VehicleFuelTypeId,
                    FipeReferenceCode = vehicle.ReferenceCode,
                    Authentication = vehicle.Autenticacao,
                    Created = vehicle.Created
                });
            }
            return vehiclesEntity;
        }
    }
}
