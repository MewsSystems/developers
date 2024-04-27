using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateFinder.ConsoleApp.ApiClients
{
    class HttpClientService
    {
        private HttpClient _httpClient;

        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetCalculatedExchangeRatesAsync(string apiUrl)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
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
