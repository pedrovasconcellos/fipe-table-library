using RestSharp;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

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

        public async Task<T> Post<T>(string endPoint, object requestObject) where T : new()
        {
            this._logger.LogDebug("Starting http[post]. Method={method}; EndPoint={endPoint}; RequestObject={@requestObject};",
                nameof(this.Post), endPoint, requestObject);

            short retry = 10;
            while (retry-- > 0)
            {
                try
                {
                    return await this.HttpPost<T>(endPoint, requestObject);
                }
                catch(Exception ex)
                {
                    this._logger.LogInformation("ThreadSleep={milliseconds} milliseconds; Retry={retry}; EndPoint={endPoint}; RequestObject={@requestObject}; Exception.Message={exMessage};",
                        this._settings.Milliseconds, retry, endPoint , requestObject, ex.Message);

                    Thread.Sleep(this._settings.Milliseconds);
                }
            }
            this._logger.LogError("Exhausted attempts. Method={method}; Retry={retry}; EndPoint={endPoint}; RequestObject={@requestObject};",
                nameof(this.Post), retry, endPoint, requestObject);

            return new T();
        }

        private async Task<T> HttpPost<T>(string endPoint, object requestObject) where T : new()
        {
            var restRequest = new RestRequest(endPoint, Method.Post);

            if(requestObject != null)
            {
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddJsonBody(requestObject);
            }

            foreach (var requestHeader in this._settings.RequestHeaders)
                restRequest.AddHeader(requestHeader.Key, requestHeader.Value);

            var result = await new RestClient(this._settings.ServiceUrl).PostAsync(restRequest);
            var resultObj = JsonConvert.DeserializeObject<T>(result.Content);
            this._logger.LogDebug($"Request response. Method={nameof(this.HttpPost)}; Response={result.Content};");

            if (resultObj == null)
                return new T();

            return resultObj;
        }
    }
}
