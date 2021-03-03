using System.Threading.Tasks;

namespace Vasconcellos.FipeTable.UploadService.Services.Interfaces
{
    public interface IFipeUploadService
    {
        Task<bool> ProcessUpload(int fipeReferenceId = 0, bool processingForce = false);
    }
}
