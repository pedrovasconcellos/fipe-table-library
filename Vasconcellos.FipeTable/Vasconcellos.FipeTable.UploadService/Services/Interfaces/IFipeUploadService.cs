using System.Threading.Tasks;

namespace Vasconcellos.FipeTable.UploadService.Services.Interfaces
{
    public interface IFipeUploadService
    {
        Task<bool> Process(bool processingForce);
    }
}
