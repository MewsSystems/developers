using ExchangeRateUpdater.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Interfaces
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
