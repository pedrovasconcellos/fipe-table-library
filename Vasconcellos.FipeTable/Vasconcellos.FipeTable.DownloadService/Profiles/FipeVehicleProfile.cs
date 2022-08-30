using System;
using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Entities.Denormalized;

namespace Vasconcellos.FipeTable.DownloadService.Profiles
{
    public static class FipeVehicleProfile
    {
        public static (List<FipeVehicleInformation> Vehicles, List<FipeVehiclePrice> Prices, List<FipeVehicleInformationDenormalized> VehiclesDenormalized)
            ModelToEntity(this IEnumerable<Vehicle> vehicles)
        {
            var vehiclesEntity = new List<FipeVehicleInformation>();
            var vehiclesDenormalizedEntity = new List<FipeVehicleInformationDenormalized>();
            var pricesEntity = new List<FipeVehiclePrice>();

            foreach (var vehicle in vehicles)
            {
                var vehicleEntity = GetFipeVehicleInformation(vehicle);
                vehiclesEntity.Add(vehicleEntity);

                var vehicleDenormalizedEntity = GetFipeVehicleInformationDenormalized(vehicle, vehicleEntity);
                vehiclesDenormalizedEntity.Add(vehicleDenormalizedEntity);

                var priceEntity = GetFipeVehiclePrice(vehicle, vehicleEntity);
                pricesEntity.Add(priceEntity);
            }

            return (vehiclesEntity, pricesEntity, vehiclesDenormalizedEntity);
        }

        private static FipeVehicleInformation GetFipeVehicleInformation(Vehicle vehicle)
        {
            return new FipeVehicleInformation(
                        vehicle.CodigoFipe,
                        Convert.ToInt64(vehicle.ModelId),
                        vehicle.Year,
                        vehicle.VehicleFuelTypeId,
                        vehicle.Autenticacao,
                        vehicle.FipeVehicleFuelTypeId
                    );
        }

        private static FipeVehicleInformationDenormalized GetFipeVehicleInformationDenormalized(
            Vehicle vehicle, FipeVehicleInformation vehicleEntity)
        {
            return new FipeVehicleInformationDenormalized(
                        vehicleEntity,
                        vehicle.TipoVeiculo,
                        Convert.ToInt64(vehicle.BrandId),
                        vehicle.Marca,
                        vehicle.Modelo,
                        vehicle.VehicleFuelTypeId,
                        vehicle.FipeVehicleFuelTypeId);
        }

        private static FipeVehiclePrice GetFipeVehiclePrice(
            Vehicle vehicle, FipeVehicleInformation vehicleEntity)
        {
            return new FipeVehiclePrice(
                    vehicle.ReferenceId,
                    vehicleEntity.Id,
                    vehicle.Value);
        }
    }
}
