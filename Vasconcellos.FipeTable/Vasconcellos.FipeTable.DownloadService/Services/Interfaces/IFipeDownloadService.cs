using System.Collections.Generic;
using System.Threading.Tasks;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Entities;

namespace Vasconcellos.FipeTable.DownloadService.Services.Interfaces
{
    public interface IFipeDownloadService
    {
        Task<List<Reference>> GetListReferenceIdFipeTable();
        Task<FipeReference> GetFipeTableReference(int requestReferenceId = 0);
        Task GetBrands(FipeDataTable fipeTable);
        Task GetModels(FipeDataTable fipeTable);
        Task GetYearsAndFuels(FipeDataTable fipeTable);
        Task<List<Vehicle>> GetVehicles(FipeDataTable fipeTable);
    }
}
