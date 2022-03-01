using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Providers;

public class NullExchangeRateProvider : IExchangeRateProvider
{
    public const string ProviderName = "Null";
    public string Name => ProviderName;
    
    public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        return Task.FromResult(Enumerable.Empty<ExchangeRate>());
    }
}