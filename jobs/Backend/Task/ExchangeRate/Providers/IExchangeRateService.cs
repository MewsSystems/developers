using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.ExchangeRate.Providers
{
    public interface IExchangeRateService
    {
        Task<List<Models.ExchangeRate>> GetExchangeRateAsync(IEnumerable<Currency> currencies);
    }

}