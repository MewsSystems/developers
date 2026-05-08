using System.Collections.Generic;
using ExchangeRateUpdater.BusinessLogic.Models;

namespace ExchangeRateUpdater.Data
{
    /// <summary>
    /// Data layer provider interface for retrieving exchange rates from a source.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Retrieves exchange rates from the data source for the specified currencies.
        /// </summary>
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
