using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies
{
    public class NullExchangeRateProviderStrategy : IExchangeRateProviderSourceCurrencyStrategy, IExchangeRateProviderTargetCurrencyStrategy
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(Currency relativeCurrency, IEnumerable<Currency> currencies)
            => Task.FromResult(Enumerable.Empty<ExchangeRate>());
    }
}
