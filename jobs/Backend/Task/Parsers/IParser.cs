using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IParser<TIn, TRes>
    {
        bool TryParse(TIn source, out IEnumerable<TRes> result);
    }
}
