using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.responses;

namespace ExchangeRateUpdater.services
{
    public interface IExchangeRateProvider
    {
       Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
