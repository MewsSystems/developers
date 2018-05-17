using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Abstract;
using ExchangeRateUpdater.Utils;
using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Cnb
{
    public class CnbRatesHttpClientFetcher : ICnbRatesFetcher
    {
        private const string CnbApiId = "CNB_API";

        public async Task<string> FetchRatesAsync(string url)
        {
            var client = HttpClientProvider.GetClient(CnbApiId);

            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching rates from CNB: {content}");
            }

            return content;
        }
    }
}
