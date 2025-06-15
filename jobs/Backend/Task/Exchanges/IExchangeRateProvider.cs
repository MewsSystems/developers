using ExchangeRateUpdater.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Exchanges;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
}
