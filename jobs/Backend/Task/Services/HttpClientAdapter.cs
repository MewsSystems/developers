using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class HttpClientAdapter: IDisposable
    {
        private HttpClient _client;

        public HttpClientAdapter()
        {
            _client = new HttpClient();
        }

        public async Task<Stream> GetStreamAsync(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}