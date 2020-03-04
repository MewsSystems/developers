using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Simple HttpClient wrapper providing retry functionality.
    /// </summary>
    class HttpClientWrapper: IDisposable
    {
        private HttpClient _client;

        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Asynchronously get content from URL. If not successful retry specified amount of times.
        /// </summary>
        /// <param name="URL">URL to get from.</param>
        /// <param name="retriesLimit">Number of times to retry the request.</param>
        /// <returns>Content from URL as stream.</returns>
        public async Task<Stream> GetStreamAsync(string URL, byte retriesLimit = 5)
        {
            var response = await _client.GetAsync(URL);
            byte attempts = 0;

            while (!response.IsSuccessStatusCode)
            {
                if (attempts < retriesLimit)
                {
                    ++attempts;
                    await Task.Delay(attempts * 1000); // after each failed request wait a little
                    response = await _client.GetAsync(URL);
                }
                else
                {
                    throw new HttpRequestException($"Unable to access \"{URL}\", status: {response.StatusCode}.");
                }
            }

            return await response.Content.ReadAsStreamAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
