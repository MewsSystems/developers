using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        return Task.FromResult(Enumerable.Empty<ExchangeRate>());
    }
}
