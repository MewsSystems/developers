using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Exchange_Providers.Models;

namespace ExchangeRateUpdater.Exchange_Providers.Interfaces
{
    /// <summary>
    /// Represents an interface for retrieving exchange rates for a set of currencies
    /// from a specific source or provider.
    /// </summary>
    internal interface IExchangeRateProvider
    {
        /// <summary>
        /// Retrieves exchange rates for a specified set of currencies.
        /// </summary>
        /// <param name="currencies">The currencies for which exchange rates are to be retrieved.</param>
        /// <param name="date">Optional. The date for which exchange rates are requested (null for the latest rates).</param>
        /// <returns>
        /// Returns a collection of ExchangeRate objects containing the requested exchange rate information.
        /// </returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? date = null);
    }
}
