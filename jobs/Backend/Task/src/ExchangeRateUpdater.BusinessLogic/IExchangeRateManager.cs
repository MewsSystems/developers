using ExchangeRateUpdater.BusinessLogic.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.BusinessLogic
{
    /// <summary>
    /// Business logic service for managing and retrieving exchange rates.
    /// </summary>
    public interface IExchangeRateManager
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source.
        /// But only those defined by the source, do not return calculated exchange rates.
        /// </summary>
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
