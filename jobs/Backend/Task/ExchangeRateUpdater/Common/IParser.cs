using ExchangeRateUpdater.Common;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Presentation
{
    internal interface IParser
    {
        IOperation Parse(IEnumerable<string> args);
    }
}
