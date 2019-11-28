using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICustomExchangeRatesProvider
    {
        /// <summary>
        /// Provides fx rates from specific source
        /// </summary>
        /// <returns>
        /// rates - all loaded rates
        /// ratesWhereUpdated - new rates where loaded from the source
        /// </returns>
        Task<(IEnumerable<ExchangeRate> rates, bool ratesWhereUpdated)> GetAllRatesAsync(); 
    }
}