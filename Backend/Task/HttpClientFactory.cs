using System;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Lazy<HttpClient> httpClient = new Lazy<HttpClient>(() => new HttpClient());

        public HttpClient Create()
        {
            return this.httpClient.Value;
        }
    }
}
