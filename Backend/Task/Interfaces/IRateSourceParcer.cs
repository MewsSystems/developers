using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IRateSourceParcer
    {
        IEnumerable<ExchangeRate> ParceRateSource(IEnumerable<string> rateSource);
    }
}
