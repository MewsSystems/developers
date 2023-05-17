using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business.Interfaces
{
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets a simple list of today's exchange rates against the Czech Koruna (CZK).
        /// </summary>
        /// <returns>A collection of ExchangeRate items.</returns>
        Task<List<ExchangeRate>> GetExchangeRatesAsync();
    }
}
