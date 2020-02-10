using RestSharp;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;

namespace Vasconcellos.FipeTable.DownloadService.Infra
{
    public class HttpRequest : IHttpRequest
    {
        private readonly ILogger _logger;
        private readonly IHttpRequestSettings _settings;
                                          
        public HttpRequest(ILogger logger, IHttpRequestSettings settings)
        {
            this._logger = logger;
            this._settings = settings;
        }

        public T Post<T>(string endPoint, object obj) where T : new()
        {
            this._logger.LogInformation("Start", $"EndPoint={endPoint}; ObjectRequest={JsonSerializer.Serialize(obj)}");
            short retry = 10;
            while (retry-- > 0)
            {
                try
                {
                    return this.HttpPost<T>(endPoint, obj);
                }
                catch
                {
                    this._logger.LogInformation("Sleep", $"ThreadSleep={this._settings.Milliseconds}; Retry={retry}; EndPoint={endPoint}; ObjectRequest={JsonSerializer.Serialize(obj)}");
                    Thread.Sleep(this._settings.Milliseconds);
                }
            }
            this._logger.LogInformation("Exhausted attempts", $"Retry={retry}; EndPoint={endPoint}; ObjectRequest={JsonSerializer.Serialize(obj)}");
            return new T();
        }

        private T HttpPost<T>(string endPoint, object obj) where T : new()
        {
            var restRequest = new RestRequest(endPoint, Method.POST)
            {
                RequestFormat = DataFormat.Json
            }
            .AddJsonBody(obj);

            foreach (var requestHeader in this._settings.RequestHeaders)
                restRequest.AddHeader(requestHeader.Key, requestHeader.Value);

            var result = new RestClient(this._settings.ServiceUrl).Post<T>(restRequest);
            this._logger.LogDebug("RestSharpResponse", JsonSerializer.Serialize(result.Data));
            if (!result.IsSuccessful)
                return new T();

            return result.Data;
        }
    }
}
