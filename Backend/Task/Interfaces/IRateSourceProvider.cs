using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IRateSourceProvider
    {
        IEnumerable<string> GetRateSourcesByUrl(IEnumerable<string> sourceUrls);
    }
}
