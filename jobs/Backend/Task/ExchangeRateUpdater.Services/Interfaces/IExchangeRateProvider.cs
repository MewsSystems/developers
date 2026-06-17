using ExchangeRateUpdater.Services.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
