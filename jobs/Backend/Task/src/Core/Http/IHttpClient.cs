using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Http
{
    public interface IHttpClient
    {
        Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken);
    }
}
