using System.Threading.Tasks;

namespace Vasconcellos.FipeTable.DownloadService.Infra.Interfaces
{
    public interface IHttpRequest
    {
        Task<T> Post<T>(string endPoint, object obj) where T : new();
    }
}
