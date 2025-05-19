using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Parsers
{
    /// <summary>
    /// Defines a contract for parsing raw data and returning it as a structured collection of exchange rates.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parses exchange rates from the raw input data string.
        /// </summary>
        /// <param name="input">The raw exchange rate data string to parse.</param>
        /// <returns>An <see cref="IEnumerable{ExchangeRate}"/> extracted from the raw data.</returns>
        public IEnumerable<ExchangeRate> ParseData(string input);
    }
}
