using System.Collections.Generic;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;

namespace Vasconcellos.FipeTable.DownloadService.Infra
{
    public class HttpRequestSettings : IHttpRequestSettings
    {
        public HttpRequestSettings(int milliseconds = 300000, string serviceUrl = "http://veiculos.fipe.org.br/api/veiculos/"
            , Dictionary<string, string> requestHeaders = null)
        {
            this.Milliseconds = milliseconds;
            this.ServiceUrl = serviceUrl;
            this.RequestHeaders = requestHeaders ?? new Dictionary<string, string>
            {
                { "Host", "veiculos.fipe.org.br" },
                { "Referer", "http://veiculos.fipe.org.br" },
                { "Content-Type", "application/json" },
            };
        }

        public int Milliseconds { get; private set; }
        public string ServiceUrl { get; private set; }
        public IReadOnlyDictionary<string, string> RequestHeaders { get; private set; }
    }
}
