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
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
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
