using RestSharp;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Vasconcellos.FipeTable.DownloadService.Infra
{
    internal class HttpRequest
    {
        private readonly ILogger _logger; 
        private const int _milliseconds = 300000;
        private const string _serviceUrl = "http://veiculos.fipe.org.br/api/veiculos/";
        private const string _host = "veiculos.fipe.org.br";
        private const string _referer = "http://veiculos.fipe.org.br";
        private const string _contentType = "application/json";

        internal HttpRequest(ILogger logger)
        {
            this._logger = logger;
        }

        internal T Post<T>(string endPoint, object obj) where T : new()
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
                    this._logger.LogInformation("Sleep", $"ThreadSleep={_milliseconds}; Retry={retry}; EndPoint={endPoint}; ObjectRequest={JsonSerializer.Serialize(obj)}");
                    Thread.Sleep(_milliseconds);
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
            .AddJsonBody(obj)
            .AddHeader("Host", _host)
            .AddHeader("Referer", _referer)
            .AddHeader("Content-Type", _contentType);

            var result = new RestClient(_serviceUrl).Post<T>(restRequest);
            this._logger.LogDebug("RestSharpResponse", JsonSerializer.Serialize(result.Data));
            if (!result.IsSuccessful)
                return new T();

            return result.Data;
        }
    }
}
