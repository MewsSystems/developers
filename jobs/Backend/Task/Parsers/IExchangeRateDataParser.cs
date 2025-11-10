using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Parsers
{
    /// <summary>
    /// Generic contract for parsing exchange rate data from any format.
    /// Implementations can handle different data formats (TXT, XML, JSON, CSV).
    /// </summary>
    public interface IExchangeRateDataParser
    {
        /// <summary>
        /// Parses raw data into structured exchange rates.
        /// </summary>
        /// <param name="rawData">Raw text data from the API</param>
        /// <param name="targetCurrencies">Currencies to filter for</param>
        /// <returns>Collection of exchange rates</returns>
        IEnumerable<ExchangeRate> Parse(string rawData, IEnumerable<Currency> targetCurrencies);
    }
}