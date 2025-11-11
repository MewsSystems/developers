using ExchangeRateUpdater.Core.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Http
{
    public class DefaultHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public DefaultHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("The request was cancelled due to timeout.");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching data from URL '{url}': {ex.Message}", ex);
            }

        }
    }
}
