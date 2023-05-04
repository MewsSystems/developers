using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
    public Task <IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);

    }
}