using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business
{
    public class CzechNationalBankService : IForeignExchangeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CzechNationalBankService> _logger;
        private readonly CzechNationalBankOptions _options;

        public CzechNationalBankService(ILogger<CzechNationalBankService> logger, HttpClient httpClient, IOptions<CzechNationalBankOptions> options)
        {
            _logger = logger;
            _httpClient = httpClient;
            _options = options?.Value;
        }

        public async Task<List<ThirdPartyExchangeRate>> GetLiveRatesAsync()
        {
            _logger.LogDebug($"Entering {nameof(GetLiveRatesAsync)}");

            if (_options == null || string.IsNullOrWhiteSpace(_options.Endpoint))
            {
                _logger.LogError("Exiting - API options are not configured");
                return null;
            }

            // Example request:
            // https://api.cnb.cz/cnbapi/exrates/daily?date=2023-05-17&lang=EN
            // The date defaults to today (yyyy-MM-dd) and the lang defaults to CZ
            var responseMessage = await _httpClient.GetAsync($"{_options.Endpoint}?lang={_options.Language.ToUpper()}");
            
            if (!responseMessage.IsSuccessStatusCode)
            {
                _logger.LogError("Exiting - Could not request live data");
                return null;
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<CzechNationalBankResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response?.Rates == null)
            {
                _logger.LogError($"Exiting - cannot deserialize {nameof(CzechNationalBankResponse)}");
                return null;
            }

            _logger.LogDebug($"Returning {response.Rates.Count} third party exchange rates");
            return response.Rates;
        }
    }
}
