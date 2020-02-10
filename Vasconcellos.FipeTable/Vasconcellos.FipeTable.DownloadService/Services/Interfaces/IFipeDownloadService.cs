using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Models.Responses;

namespace Vasconcellos.FipeTable.DownloadService.Services.Interfaces
{
    public interface IFipeDownloadService
    {
        int GetCodeTableReference(int requestReferenceCode = 0);
        void GetBrands(FipeDataTable fipeTable);
        void GetModels(FipeDataTable fipeTable);
        void GetYearsAndFuels(FipeDataTable fipeTable);
        List<Vehicle> GetVehicles(FipeDataTable fipeTable);
    }
}
