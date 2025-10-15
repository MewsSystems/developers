using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public interface ICnbClient
{
    Task<string> GetDailyRatesAsync(CancellationToken ct);
}
