using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Helpers
{
    public class HttpHelperClient
    {
        // To avoid port exhaustion
        private readonly static HttpClient _client = new HttpClient();

        public async Task<string> GetUrl(string url)
        {
            string content = string.Empty;

            try
            {
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // TODO: Log exception to some logger
                throw;
            }

            return content;
        }

    }
}
