using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Clients
{
    public abstract class BaseApiClient
    {
        protected BaseApiClient() { }

        public abstract HttpClient GetClient();

        protected async Task<HttpResponseMessage> Execute<T>(HttpMethod method, Uri uri, T body)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = method,
                Content = GetString(body)
            };

            return await SendRequest(request);
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            using (request)
            {
                return await GetClient().SendAsync(request);
            }
        }

        private static StringContent GetString<T>(T body)
        {
            return body != null ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json") : null;
        }
    }
}