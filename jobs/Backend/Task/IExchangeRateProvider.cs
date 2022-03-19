using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    ///   Exchange Rate Provider Interface
    /// </summary>
    internal interface IExchangeRateProvider
    {

        /// <summary>Gets the exchange rates.</summary>
        /// <param name="currencies">Requested currencies</param>
        /// <returns>Exchange rates of requested curencies</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
