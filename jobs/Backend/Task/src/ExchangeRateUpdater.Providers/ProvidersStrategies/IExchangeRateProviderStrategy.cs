using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers.ProvidersStrategies
{
    public interface IExchangeRateProviderStrategy
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
