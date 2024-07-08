using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater;


namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}

