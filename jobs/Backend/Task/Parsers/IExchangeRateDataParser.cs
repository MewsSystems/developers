namespace ExchangeRateUpdater.Parsers
{
    using ExchangeRateUpdater.Models;
    using System.Collections.Generic;

    public interface IExchangeRateDataParser
    {
        IEnumerable<ExchangeRate> Parse(string data, IEnumerable<Currency> currencies);
    }
}
