using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    internal class FixexApi : IApiWrapper
    {
        private readonly HttpClient _client;

        private const string Url = "http://api.fixer.io/latest?base={0}&symbols={1}";
        private const string InvalidCurrencyCode = "422";

        public FixexApi()
        {
            _client = new HttpClient();
        }

        public async Task<IRate> Get(string baseCurrency, HashSet<Currency> currencies)
        {
            FixerRates fixerRates;

            var targetCurrencies = string.Join(",", currencies.Select(c => c.Code));

            var response = await _client.GetAsync(string.Format(Url, baseCurrency, targetCurrencies)).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                fixerRates = await response.Content.ReadAsAsync<FixerRates>().ConfigureAwait(false);
            }
            else
            {
                if (response.StatusCode.ToString() == InvalidCurrencyCode)
                {
                    return await Task.FromResult<FixerRates>(null).ConfigureAwait(false);
                }
                throw new Exception($"API Error {(int)response.StatusCode}");
            }

            return fixerRates;
        }
    }
}
