using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICustomExchangeProvider
    {
        Task<IEnumerable<ExchangeRate>> GetData(string baseCurrencyCode, IEnumerable<Currency> currencies);
    }
}
