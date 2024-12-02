using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Services
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _client;

        public HttpClientWrapper(HttpClient client)
        {
            _client = client;
        }

        public Task<string> GetStringAsync(string url)
        {
            return _client.GetStringAsync(url);
        }
    }
}
