using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IExchangeRateProviderService
    {
        Task<IEnumerable<ExchangeRate>> GetCNBExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}
