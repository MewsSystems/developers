using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRatesSource
    {
        Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(Currency baseCurrency, IEnumerable<Currency> requestedCurrencies);
    }
}