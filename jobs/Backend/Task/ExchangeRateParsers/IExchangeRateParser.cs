using System.Collections.Generic;
using System.IO;

namespace ExchangeRateUpdater.ExchangeRateParser
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRateParceItem> Parce(Stream data);
    }
}
