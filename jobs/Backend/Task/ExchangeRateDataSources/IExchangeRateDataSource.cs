using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateDataProviders
{
    public interface IExchangeRateDataSource
    {
        Task<Stream> GetDataAsync(CancellationToken ct);
    }
}
