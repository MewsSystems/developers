using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Business.Interfaces
{
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets a simple list of exchange rates from a data source 
        /// as defined in the app configuration.
        /// </summary>
        /// <returns>A collection of ExchangeRate items.</returns>
        IEnumerable<ExchangeRate> GetExchangeRates();
    }
}
