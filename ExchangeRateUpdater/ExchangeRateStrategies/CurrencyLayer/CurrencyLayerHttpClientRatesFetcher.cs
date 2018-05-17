using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Abstract;
using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer.Model;
using ExchangeRateUpdater.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer
{
    public class CurrencyLayerHttpClientRatesFetcher : ICurrencyLayerRatesFetcher
    {
        private const string CurrencyLayerApiId = "CURRENCY_LAYER_API";

        public async Task<CurrencyLayerResponse> FetchRatesAsync(string apiKey, IEnumerable<string> currencies)
        {
            var client = HttpClientProvider.GetClient(CurrencyLayerApiId);

            var url = ComposeUrl(apiKey, currencies);
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to fetch data from CurrencyLayer API: {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<CurrencyLayerResponse>(json);
            if (!data.Success)
            {
                throw new Exception($"Error fetching data from CurrencyLayer API: {data.Error?.Code} - {data.Error?.Type}: {data.Error?.Info ?? "N/A"}");
            }

            return data;
        }

        private string ComposeUrl(string apiKey, IEnumerable<string> currencies)
            => $"http://apilayer.net/api/live?access_key={apiKey}&currencies={string.Join(",", currencies)}";
    }
}
