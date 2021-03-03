using System;
using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Entities;

namespace Vasconcellos.FipeTable.DownloadService.Profiles
{
    public static class FipeVehicleProfile
    {
        public static (List<FipeVehicleInformation> Vehicles, List<FipeVehiclePrice> Prices) 
            ModelToEntity(this IEnumerable<Vehicle> vehicles)
        {
            var vehiclesEntity = new List<FipeVehicleInformation>();
            var pricesEntity = new List<FipeVehiclePrice>();

            foreach (var vehicle in vehicles)
            {
                var vehicleEntity = new FipeVehicleInformation(
                        vehicle.CodigoFipe,
                        Convert.ToInt64(vehicle.ModelId),
                        vehicle.Year,
                        vehicle.VehicleFuelTypeId,
                        vehicle.Autenticacao,
                        vehicle.FipeVehicleFuelTypeId
                    );

                vehiclesEntity.Add(vehicleEntity);

                pricesEntity.Add(new FipeVehiclePrice(
                    vehicle.ReferenceId, 
                    vehicleEntity.Id, 
                    vehicle.Value));
            }
            return (vehiclesEntity , pricesEntity);
        }
    }
}
