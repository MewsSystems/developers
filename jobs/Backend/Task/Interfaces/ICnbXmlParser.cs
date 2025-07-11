using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    /// <summary>
    /// Interface for parsing XML containing exchange rate data.
    /// </summary>
    public interface ICnbXmlParser
    {
        IEnumerable<ExchangeRate> Parse(string xml, IEnumerable<Currency> requestedCurrencies);
    }
}
