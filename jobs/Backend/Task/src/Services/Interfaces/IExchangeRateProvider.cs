using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services.Interfaces
{
    /// <summary>
    /// Provider for exchange rates from any source
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets exchange rates for the specified currencies
        /// </summary>
        /// <param name="currencies">The currencies to get rates for</param>
        /// <returns>Collection of exchange rates</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);

        /// <summary>
        /// Updates the cached exchange rates from the source
        /// </summary>
        Task UpdateRatesAsync();
    }
} 