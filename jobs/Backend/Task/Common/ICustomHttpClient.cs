using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Common
{
    public interface ICustomHttpClient
    {
        Task<T> GetAsync<T>(string url, CancellationToken cancellationToken);
    }
}
