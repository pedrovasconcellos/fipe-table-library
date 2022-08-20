using System.Collections.Generic;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Entities.Denormalized;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads
{
    public class NormalizedDownloadResult
    {
        public NormalizedDownloadResult(
            FipeReference fipeReference, 
            FipeVehicleTypesEnum vehicleType, 
            List<FipeVehicleBrand> brands,
            List<FipeVehicleModel> models, 
            List<FipeVehicleInformation> vehicles, 
            List<FipeVehiclePrice> prices,
            List<FipeVehicleInformationDenormalized> vehiclesDenormalized)
        {
            this.FipeReference = fipeReference;
            this.VehicleType = vehicleType;
            this.Brands = brands.AsReadOnly();
            this.Models = models.AsReadOnly();
            this.Vehicles = vehicles.AsReadOnly();
            this.Prices = prices.AsReadOnly();
            this.VehiclesDenormalized = vehiclesDenormalized.AsReadOnly();
        }

        public FipeReference FipeReference { get; private set; }
        public FipeVehicleTypesEnum VehicleType { get; private set; }
        public IList<FipeVehicleBrand> Brands { get; private set; }
        public IList<FipeVehicleModel> Models { get; private set; }
        public IList<FipeVehicleInformation> Vehicles { get; private set; }
        public IList<FipeVehiclePrice> Prices { get; private set; }
        public IList<FipeVehicleInformationDenormalized> VehiclesDenormalized { get; private set; }
    }
}
