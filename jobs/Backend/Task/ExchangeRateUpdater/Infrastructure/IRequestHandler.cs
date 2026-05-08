using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

internal interface IRequestHandler
{
    Task<Stream> GetStreamAsync(string requestUri);
}
