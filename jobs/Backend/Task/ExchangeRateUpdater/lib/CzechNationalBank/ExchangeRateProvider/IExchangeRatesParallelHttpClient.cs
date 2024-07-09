using ExchangeRateUpdater.Lib.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    public interface IExchangeRatesParallelHttpClient
    {
        Task<IEnumerable<ProviderExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}