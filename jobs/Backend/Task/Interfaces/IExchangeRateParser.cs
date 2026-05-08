using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> Parse(string data, IEnumerable<Currency> currencies);
    }
}
