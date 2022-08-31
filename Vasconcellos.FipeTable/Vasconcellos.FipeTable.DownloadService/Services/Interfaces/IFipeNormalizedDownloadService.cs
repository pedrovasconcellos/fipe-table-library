using System.Threading.Tasks;
using Vasconcellos.FipeTable.DownloadService.Models.NormalizedDownloads;
using Vasconcellos.FipeTable.Types.Enums;

namespace Vasconcellos.FipeTable.DownloadService.Services.Interfaces
{
    public interface IFipeNormalizedDownloadService
    {
        Task<NormalizedDownloadResult> GetDataFromFipeTableByVehicleType(FipeVehicleTypesEnum vehicleType, int referenceId = 0);
    }
}