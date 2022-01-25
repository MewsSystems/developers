using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders.QuotesProviders
{
    public class WebProxyProvider : IWebProxyProvider
    {
        public async Task<string> GetUrlAsync(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException(nameof(url));

            using (HttpClient client = new HttpClient())
                return await client.GetStringAsync(url);
        }
    }
}
