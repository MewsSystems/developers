using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Repositories;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository)
    {

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = await exchangeRateRepository.FilterAsync(currencies);
            return exchangeRates.Count == 0 ? [] : exchangeRates.First().Value;
        }
        
        /// <summary>
        /// Returns a dictionary of exchange rates for each provider.
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, ExchangeRate[]>> GetExchangeRatesFromMultipleProviders(IEnumerable<Currency> currencies)
        {
            return await exchangeRateRepository.FilterAsync(currencies);
        }
    }
}
