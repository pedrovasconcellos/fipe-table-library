using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;
using Vasconcellos.FipeTable.Types.Entities;

namespace Vasconcellos.FipeTable.DownloadService.Services.Interfaces
{
    public interface IFipeDownloadService
    {
        List<Reference> GetListReferenceIdFipeTable();
        FipeReference GetFipeTableReference(int requestReferenceId = 0);
        void GetBrands(FipeDataTable fipeTable);
        void GetModels(FipeDataTable fipeTable);
        void GetYearsAndFuels(FipeDataTable fipeTable);
        List<Vehicle> GetVehicles(FipeDataTable fipeTable);
    }
}
