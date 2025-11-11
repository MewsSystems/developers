using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain
{
    internal interface IExchangeRateFetcher
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(CancellationToken cancellationToken = default);
    }
}
