using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business
{
    public interface IExchangeRateProvider
    {
        Task<List<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}