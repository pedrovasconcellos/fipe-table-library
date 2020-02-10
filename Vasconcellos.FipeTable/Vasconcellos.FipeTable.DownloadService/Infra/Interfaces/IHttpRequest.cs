
namespace Vasconcellos.FipeTable.DownloadService.Infra.Interfaces
{
    public interface IHttpRequest
    {
        T Post<T>(string endPoint, object obj) where T : new();
    }
}
