using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IParser
    {
        Dictionary<string, decimal> Parse(string txtContent);
    }
}