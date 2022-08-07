using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateDataProviders
{
    public interface IExchangeRateDataSource
    {
        Task<string> GetDataAsync(CancellationToken ct);
    }
}
