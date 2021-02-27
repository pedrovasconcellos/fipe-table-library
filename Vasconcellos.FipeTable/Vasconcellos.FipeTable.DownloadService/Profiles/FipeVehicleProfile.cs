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
                vehiclesEntity.Add(new FipeVehicleInformation(
                        Convert.ToInt64(vehicle.BrandId),
                        vehicle.CodigoFipe,
                        vehicle.ReferenceCode,
                        Convert.ToInt64(vehicle.ModelId),
                        vehicle.Year,
                        vehicle.VehicleFuelTypeId,
                        vehicle.Value,
                        vehicle.Autenticacao,
                        vehicle.FipeVehicleFuelTypeId
                    ));
            }
            return vehiclesEntity;
        }
    }
}
