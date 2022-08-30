using System.Collections.Generic;
using System.Threading.Tasks;
using Vasconcellos.FipeTable.Types.Entities;
using Vasconcellos.FipeTable.Types.Entities.Denormalized;

namespace Vasconcellos.FipeTable.UploadService.Domains.Interfaces
{
    public interface IFipeUploadDomain
    {
        bool HaveReferenceIdGreaterOrEquals(long referenceId);
        Task<bool> SaveFipeReference(FipeReference fipeReference);
        Task<bool> SaveVehicleBrands(IList<FipeVehicleBrand> brands);
        Task<bool> SaveVehicleModels(IList<FipeVehicleModel> models);
        Task<bool> SaveVehicles(IList<FipeVehicleInformation> vehicles);
        Task<bool> SavePrices(IList<FipeVehiclePrice> prices);
        Task<bool> SaveVehiclesDenormalized(IList<FipeVehicleInformationDenormalized> vehicles);
    }
}
