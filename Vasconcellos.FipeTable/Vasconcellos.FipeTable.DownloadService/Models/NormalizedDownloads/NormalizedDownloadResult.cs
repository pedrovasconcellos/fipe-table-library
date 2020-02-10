using System.Collections.Generic;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads
{
    public class NormalizedDownloadResult
    {
        public NormalizedDownloadResult(int referenceCode, FipeVehicleTypesEnum vehicleType, List<FipeVehicleBrand> brands,
            List<FipeVehicleModel> models, List<FipeVehicleInformation> vehicles)
        {
            this.ReferenceCode = referenceCode;
            this.VehicleType = vehicleType;
            this.Brands = brands.AsReadOnly();
            this.Models = models.AsReadOnly();
            this.Vehicles = vehicles.AsReadOnly();
        }

        public int ReferenceCode { get; private set; }
        public FipeVehicleTypesEnum VehicleType { get; set; }
        public IList<FipeVehicleBrand> Brands { get; private set; }
        public IList<FipeVehicleModel> Models { get; private set; }
        public IList<FipeVehicleInformation> Vehicles { get; private set; }
    }
}
