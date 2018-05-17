using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies
{
    public interface IExchangeRateProviderStrategy
    {
        /// <summary>
        /// Provides exchange rates for requested relative currency and one or more other currencies
        /// </summary>
        /// <param name="relativeCurrency">Relative currency - source or target</param>
        /// <param name="currencies">Currencies for which the rates should be provided</param>
        /// <returns>List of exchange rates</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(Currency relativeCurrency, IEnumerable<Currency> currencies);
    }
}
