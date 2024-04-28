using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateFinder.ConsoleApp.ApiClients
{
    public class HttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ExternalApi");
                var response = await client.GetAsync(endpoint);
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
