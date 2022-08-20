using RestSharp;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vasconcellos.FipeTable.DownloadService.Infra.Interfaces;
using System;

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

        public T Post<T>(string endPoint, object requestObject) where T : new()
        {
            this._logger.LogDebug("Starting http[post]. Method={method}; EndPoint={endPoint}; RequestObject={@requestObject};",
                nameof(this.Post), endPoint, requestObject);

            short retry = 10;
            while (retry-- > 0)
            {
                try
                {
                    return this.HttpPost<T>(endPoint, requestObject);
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

        private T HttpPost<T>(string endPoint, object requestObject) where T : new()
        {
            var restRequest = new RestRequest(endPoint, Method.POST)
            {
                RequestFormat = DataFormat.Json
            }
            .AddJsonBody(requestObject);

            foreach (var requestHeader in this._settings.RequestHeaders)
                restRequest.AddHeader(requestHeader.Key, requestHeader.Value);

            var result = new RestClient(this._settings.ServiceUrl).Post<T>(restRequest);
            this._logger.LogDebug("Request response. Method={method}; ResponseObject={@responseObject};",
                nameof(this.HttpPost), result.Data);

            if (!result.IsSuccessful)
                return new T();

            return result.Data;
        }
    }
}
