using System;
using ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Abstract;
using ExchangeRateUpdater.ExchangeRateStrategies.Fixer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Utils;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Fixer
{
    public class FixerHttpClientRatesFetcher : IFixerRatesFetcher
    {
        private const string FixerApiId = "FIXER_API";

        public async Task<FixerResponse> FetchRatesAsync(string apiKey, IEnumerable<string> currencies)
        {
            var client = HttpClientProvider.GetClient(FixerApiId);

            var url = ComposeUrl(apiKey, currencies);
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to fetch data from Fixer API: {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<FixerResponse>(json);
            if (!data.Success)
            {
                throw new Exception($"Error fetching data from Fixer API: {data.Error?.Code} - {data.Error?.Type}: {data.Error?.Info ?? "N/A"}");
            }

            return data;
        }

        private string ComposeUrl(string apiKey, IEnumerable<string> currencies)
            => $"http://data.fixer.io/api/latest?access_key={apiKey}&symbols={string.Join(",", currencies)}";
    }
}
