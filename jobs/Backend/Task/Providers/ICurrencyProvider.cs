using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Providers
{
    public interface ICurrencyProvider
    {
        /// <summary>
        /// Returns the list of currencies defined in the configuration.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Currency> Get();
    }
}
