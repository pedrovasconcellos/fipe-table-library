using Vasconcellos.FipeTable.Types.Enums;
using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class FipeDataTable
    {
        public FipeDataTable(int referenceCode, FipeVehicleTypesEnum vehicleType)
        {
            this.ReferenceCode = referenceCode;
            this.VehicleType = vehicleType;
            this.Brands = new List<Brand>();
        }

        public int ReferenceCode { get; private set; }
        public FipeVehicleTypesEnum VehicleType { get; private set; }
        public List<Brand> Brands { get; set; }
    }
}
