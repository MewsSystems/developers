using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Parsers
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> Parse(string content, IEnumerable<Currency> filter, Currency source);
    }
}
