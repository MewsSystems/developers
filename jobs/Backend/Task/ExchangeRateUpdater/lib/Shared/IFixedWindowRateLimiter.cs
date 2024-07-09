using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Lib.Shared
{
    public interface IFixedWindowRateLimiter
    {
        Task<bool> WaitAsync(CancellationToken cancellationToken = default);
    }
}