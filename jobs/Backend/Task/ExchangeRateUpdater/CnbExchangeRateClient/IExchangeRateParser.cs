using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public interface IExchangeRateParser
{
    Task<IEnumerable<ExchangeRateLine>> ParseExchangeRateList(Stream stream);
}