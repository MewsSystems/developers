using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeService
    {
        Task<IEnumerable<ExchangeRateRecord>> GetExchangeRatesAsync();
    }
}
