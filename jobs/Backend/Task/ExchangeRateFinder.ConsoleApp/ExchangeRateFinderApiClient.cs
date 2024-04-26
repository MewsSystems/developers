using ExchangeRateFinder.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateFinder
{
    class ExchangeRateFinderApiClient
    {
        private HttpClient _httpClient;

        public ExchangeRateFinderApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ExchangeRateResponse>> CallApiAsync(string apiUrl)
        {
            try { 
            
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var  json =  await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<ExchangeRateResponse>>(json);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to call API: {ex.Message}");
            }
        }
    }
}
