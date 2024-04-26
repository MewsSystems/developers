using ExchangeRateFinder.ConsoleApp.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateFinder.ConsoleApp.ApiClients
{
    class ExchangeRateFinderApiClient
    {
        private HttpClient _httpClient;

        public ExchangeRateFinderApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<CalculatedExchangeRateResponse>> GetCalculatedExchangeRatesAsync(string apiUrl)
        {
            try
            {

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<CalculatedExchangeRateResponse>>(json);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to call API: {ex.Message}");
            }
        }
    }
}
