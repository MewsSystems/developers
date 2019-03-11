using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private readonly HttpClient _httpClient;
        private readonly IExchangeRateResponseParser _exchangeRateResponseParser;
        private readonly IExchangeRatesFilter _exchangeRatesFilter;
        private readonly string _url;
        private readonly Currency _targetCurrency;

        public ExchangeRateProvider(IExchangeRateResponseParser parser, IExchangeRatesFilter filter, 
            HttpClient httpClient, string url, Currency targetCurrency)
        {
            _exchangeRateResponseParser = parser;
            _exchangeRatesFilter = filter;
            _httpClient = httpClient;
            _url = url;
            _targetCurrency = targetCurrency;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }
            var contentResponse = await GetExchangeRatesResponseAsync(_url);
            var unfilteredRates = _exchangeRateResponseParser.ParseResponse(contentResponse, _targetCurrency);
            var rates = _exchangeRatesFilter.GetFilteredRates(unfilteredRates, currencies);
            return rates;
        }

        private async Task<string> GetExchangeRatesResponseAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            throw new WebException(response.ReasonPhrase);
        }

    }
}
