using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateFinder.ConsoleApp.ApiClients
{
    class HttpClientService
    {
        private HttpClient _httpClient;
        public HttpClientService(IHttpClientFactory _httpClientFactory)
        {
            _httpClient = _httpClientFactory.CreateClient();
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to call API: {ex.Message}");
            }
        }
    }
}
