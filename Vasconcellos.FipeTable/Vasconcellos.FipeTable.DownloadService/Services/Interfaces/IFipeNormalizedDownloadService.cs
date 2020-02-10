using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Services.Interfaces
{
    public interface IFipeNormalizedDownloadService
    {
        NormalizedDownloadResult GetDataFromFipeTableByVehicleType(FipeVehicleTypesEnum vehicleType, int referenceCode = 0);
    }
}