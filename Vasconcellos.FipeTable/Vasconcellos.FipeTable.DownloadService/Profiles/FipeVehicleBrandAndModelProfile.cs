using System;
using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Entities;

namespace Vasconcellos.FipeTable.DownloadService.Profiles
{
    internal static class FipeVehicleBrandAndModelProfile
    {
        internal static (List<FipeVehicleBrand> Brands, List<FipeVehicleModel> Models) ModelToEntity(this FipeDataTable fipeTable)
        {
            var brandsEntities = new List<FipeVehicleBrand>();
            var modelsEntities = new List<FipeVehicleModel>();

            foreach (var brand in fipeTable.Brands)
            {
                brandsEntities.Add(new FipeVehicleBrand()
                {
                    Id = Convert.ToInt64(brand.Value),
                    Type = fipeTable.VehicleType,
                    Description = brand.Label
                });

                if (brand.Models != null)
                    foreach (var model in brand.Models)
                    {
                        modelsEntities.Add(new FipeVehicleModel()
                        {
                            Id = Convert.ToInt64(model.Value),
                            Description = model.Label,
                            BrandId = Convert.ToInt32(brand.Value),
                        });
                    }
            }

            return (Brands: brandsEntities, Models: modelsEntities);
        }
    }
}
