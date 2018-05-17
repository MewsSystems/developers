using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Abstract
{
    public interface ICurrencyLayerRatesFetcher
    {
        Task<CurrencyLayerResponse> FetchRatesAsync(string apiKey, IEnumerable<string> currencies);
    }
}
