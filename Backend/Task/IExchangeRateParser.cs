using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateParser
    {
        IEnumerable<ExchangeRate> Parse();
    }
}
