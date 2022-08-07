using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateParser
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRateParceItem> Parce(string data);
    }
}
