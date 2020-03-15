using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}