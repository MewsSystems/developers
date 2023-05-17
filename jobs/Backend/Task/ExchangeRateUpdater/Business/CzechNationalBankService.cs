using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
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
            
            var response = await _httpClient.GetAsync(_options.Endpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Exiting - Could not request live data");
                return null;
            }

            var data = await response.Content.ReadAsStringAsync();
            var rates = CzechNationalBankHelper.ConvertToThirdPartyExchangeRates(data);

            _logger.LogDebug($"Returning {rates.Count} third party exchange rates");
            return rates;
        }
    }
}
