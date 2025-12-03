using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    /// <summary>
    /// Defines a contract for retrieving exchange rates for the specified currencies from an external data source.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Retrieves exchange rates for the given list of currencies.
        /// </summary>
        /// 
        /// <param name="currencies">
        /// The list of currencies for which exchange rates should be retrieved.
        /// Only currencies available in the underlying data source will be returned.
        /// </param>
        /// 
        /// <returns>
        /// A collection of exchange rates for the currencies that are available in the underlying data source.
        /// Currencies that are not provided by the source are excluded from the result.
        /// </returns>
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}