using System.Collections.Generic;

using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Parsers;

public interface ICurrencyParser
{
    /// <summary>
    /// Parse currencies from string
    /// </summary>
    /// <param name="data">Currency data in string format</param>
    IReadOnlyCollection<Currency> ParseCurrencies(string data);
}