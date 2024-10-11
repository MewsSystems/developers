using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Provider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        readonly IExchangeRateClient _exchangeRateClient;

        public ExchangeRateProvider(IExchangeRateClient exchangeRateClient) 
        { 
            _exchangeRateClient = exchangeRateClient;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exhangeRateEntities = await _exchangeRateClient.GetExchangeRateEntitiesAsync(currencies);
            var targetCurrency = new Currency(ExchangeRateSettings.TargetCurrency);
            var result = exhangeRateEntities
                .Where(e => currencies.Select(c => c.Code).Contains(e.CurrencyCode))
                .Select(e => new ExchangeRate(new Currency(e.CurrencyCode), targetCurrency, e.Rate));

						return result;
        }
    }
}
