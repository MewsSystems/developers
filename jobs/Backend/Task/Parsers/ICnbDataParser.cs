using System.Collections.Generic;
using ExchangeRateUpdater.Parsers.Models;

namespace ExchangeRateUpdater.Parsers
{
    // Interface for parsing CNB exchange rate data.
    public interface ICnbDataParser
    {
        // Method's signature: parses raw CNB exchange rate data and returns a collection of parsed exchange rate records
        IEnumerable<CnbExchangeRateData> Parse(string rawData);
    }
}
