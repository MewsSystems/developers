using Newtonsoft.Json;
using System.Text;

namespace Mews.ExchangeRateUpdater.Services.Infrastructure
{
    /// <summary>
    /// This is the implementation, to use HttpClient to call the external provider API to fetch and read
    /// the exchange rates response
    /// </summary>
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> Get<T>(string endpointUrl, Dictionary<string, object> parameters)
        {
            var requestMessage = GetRequest(endpointUrl + (parameters.Count > 0 ? "?" : string.Empty) + BuildQueryString(parameters), HttpMethod.Get);

            var response = await Send(requestMessage);

            response.EnsureSuccessStatusCode();

            return await ReadResponse<T>(response);            
        }

        public async Task<HttpResponseMessage> Get(string endpointUrl, Dictionary<string, object> parameters)
        {
            var requestMessage = GetRequest(endpointUrl + (parameters.Count > 0 ? "?" : string.Empty) + BuildQueryString(parameters), HttpMethod.Get);

            var response = await Send(requestMessage);

            return response;
        }

        public async Task<T?> ReadResponse<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T?>(json);
        }

        private string BuildQueryString(Dictionary<string, object> query)
        {
            if (query == null || query.Count == 0)
                return string.Empty;

            var queryBuilder = new StringBuilder();

            foreach (var item in query)
                queryBuilder.AppendFormat("{0}={1}&", item.Key, item.Value);

            var queryString = queryBuilder.ToString().Trim('&');

            return queryString;
        }

        private HttpRequestMessage GetRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, url);

            return request;
        }

        private async Task<HttpResponseMessage> Send(HttpRequestMessage message)
        {
            var response = await _httpClient.SendAsync(message);

            //response.EnsureSuccessStatusCode();

            return response;
        }

        //private async Task<T?> ReadResponse<T>(HttpResponseMessage response)
        //{
        //    var json = await response.Content.ReadAsStringAsync();

        //    return JsonConvert.DeserializeObject<T?>(json);
        //}
    }
}
