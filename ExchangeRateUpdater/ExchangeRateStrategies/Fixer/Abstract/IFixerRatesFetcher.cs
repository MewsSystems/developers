using ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Abstract
{
    public interface IFixerRatesFetcher
    {
        Task<FixerResponse> FetchRatesAsync(string apiKey, IEnumerable<string> currencies);
    }
}
