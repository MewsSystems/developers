using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Cache containing current information about available exchange rates.
    /// </summary>
    public interface IExchangeRatesListingsCache
    {
        /// <summary>
        /// Returns latest available exchange rates.
        /// </summary>
        /// <returns>List of exchange rates.</returns>
        Task<IReadOnlyList<ExchangeRate>> GetCurrentExchangeRates();
    }

}
