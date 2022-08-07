using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateParser
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> Parce(string data);
    }
}
