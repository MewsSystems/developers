using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateParser
{
    public Task<IEnumerable<ExchangeRate>> ParseAsync(Stream values);
}