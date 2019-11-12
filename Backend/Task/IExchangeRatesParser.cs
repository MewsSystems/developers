using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Defines a method for parsing string document with FX rates to IEnumerable of <see cref="ExchangeRate"/>.
    /// </summary>
    public interface IExchangeRatesParser
    {
        /// <summary>
        /// Parses string containing FX rates.
        /// </summary>
        /// <param name="fxRatesRaw">The <see cref="string"/> to try to parse.</param>
        /// <returns>An IEnumerable of the <see cref="ExchangeRate"/> class representing the list of parsed FX rates.</returns>
        IEnumerable<ExchangeRate> ParseExchangeRates(string fxRatesRaw);
    }
}
