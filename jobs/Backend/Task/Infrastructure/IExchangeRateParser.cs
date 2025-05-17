using System.Collections.Generic;

namespace ExchangeRateUpdater.Infrastructure
{
    internal interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> Parse(string exchangeRate);
    }
}
