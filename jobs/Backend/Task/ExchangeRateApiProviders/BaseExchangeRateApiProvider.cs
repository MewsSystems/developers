using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRatesDictionary
    {
        public ExchangeRatesDictionary(Dictionary<string, Dictionary<string, decimal>>? exchangeRates)
        {
            ExchangeRates = exchangeRates ?? new Dictionary<string, Dictionary<string, decimal>>();
        }

        public Dictionary<string, Dictionary<string, decimal>> ExchangeRates { get; }

        public decimal? GetRate(string sourceCurrency, string targetCurrency)
        {
            if (ExchangeRates.TryGetValue(sourceCurrency, out var sourceRates))
            {
                if (sourceRates.TryGetValue(targetCurrency, out var rate))
                {
                    return rate;
                }
            }

            return null;
        }
    }

    public abstract class BaseExchangeRateApiProvider<T>
    {
        public virtual async Task<ExchangeRatesDictionary> FetchRates()
        {
            T response = await PerformApiRequest();
            
            return TransformToDictionary(response);
        }

        public abstract string ApiEndpoint { get; }

        private static readonly HttpClient client = new HttpClient();

        protected virtual async Task<T> PerformApiRequest()
        {
            var response = await client.GetAsync(ApiEndpoint);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<T>();

            return responseBody;
        }

        protected abstract ExchangeRatesDictionary TransformToDictionary(T response);
    }
}
