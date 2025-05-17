using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
