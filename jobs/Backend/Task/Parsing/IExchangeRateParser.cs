using System.Collections.Generic;
namespace ExchangeRateUpdater.Parsing
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> Parse(string rawData);
    }
}