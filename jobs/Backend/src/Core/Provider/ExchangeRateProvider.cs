using Core.Models;
using ExchangeRateUpdater.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Client.Provider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private ILogger _logger;
        private IConfiguration _configuration;
        private IClient _client;

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, IConfiguration configuration, IClient client)
        {
            _logger = logger;
            _configuration = configuration;
            _client = client;
        }

        /// <summary>
        /// Get rates for the specified currencies
        /// </summary>
        /// <param name="currencies">Currencies to fetch rates for</param>
        /// <returns>Matching list of currencies that have rates</returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = Enumerable.Empty<ExchangeRate>();

            try
            {
                var rateData = await _client.GetExchangeRates();

                _logger.LogDebug($"Rate data \r\n");
                foreach (var data in rateData.ToList())
                {
                    _logger.LogDebug($"{data}");
                }

                rates = rateData.Where(rates => currencies.Any(currency => currency.Code.Equals(rates.SourceCurrency.Code)));
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve and parse exchange rates: {Environment.NewLine}");
                _logger.LogError(ex, ex.Message);
            }

            return rates;
        }
    }
}
