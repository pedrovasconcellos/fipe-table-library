using Vasconcellos.FipeTable.Types.Enums;
using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class FipeDataTable
    {
        public FipeDataTable(int referenceId, FipeVehicleTypesEnum vehicleType)
        {
            this.ReferenceId = referenceId;
            this.VehicleType = vehicleType;
            this.Brands = new List<Brand>();
        }

        public int ReferenceId { get; private set; }
        public FipeVehicleTypesEnum VehicleType { get; private set; }
        public List<Brand> Brands { get; set; }
    }
}
