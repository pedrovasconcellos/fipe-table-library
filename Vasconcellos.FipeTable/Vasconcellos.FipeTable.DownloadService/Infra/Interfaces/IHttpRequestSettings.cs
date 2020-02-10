using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Infra.Interfaces
{
    public interface IHttpRequestSettings
    {
        int Milliseconds { get; }
        string ServiceUrl { get; }
        IReadOnlyDictionary<string, string> RequestHeaders { get; }
    }
}
