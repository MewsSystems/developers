using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IExchangeRateProviderService
    {
        IEnumerable<Row> GetExchangeRatesAsync(IEnumerable<Currency> currencies, ExchangeRate rates);
    }
}
