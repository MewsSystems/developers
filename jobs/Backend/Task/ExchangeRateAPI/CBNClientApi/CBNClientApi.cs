using ExchangeRateUpdater.ExchangeRateAPI.DTOs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateAPI.CBNClientApi
{
    public class CBNClientApi : ICBNClientApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CBNClientApi> _logger;

        public CBNClientApi(HttpClient httpClient, ILogger<CBNClientApi> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ExchangeRatesResponseDTO> GetExratesDaily()
        {
            // API: Date in ISO format (yyyy-MM-dd); default value: NOW
            // Language enumeration; default value: CZ
            var requestUrl = "/cnbapi/exrates/daily?lang=EN";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"While fetching the data from the API status code {response.StatusCode} was returned.");

                    throw new Exception("Request did not return success status code.");
                }

                var resultAsString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ExchangeRatesResponseDTO>(resultAsString);

                _logger.LogInformation($"API returned currencies: {string.Join(",", result?.Rates?.Select(x => x.CurrencyCode).ToList())}.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the data from API.");
                throw;
            }
        }
    }
}
