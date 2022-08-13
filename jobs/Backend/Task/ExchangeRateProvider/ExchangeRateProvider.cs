using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ExchangeRateUpdater.Support;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _client;
        private readonly IRequestFactory _requestFactory;
        private readonly IParser _parser;

        public ExchangeRateProvider(HttpClient client, IRequestFactory requestFactory, IParser parser)
        {
            _client = client;
            _requestFactory = requestFactory;
            _parser = parser;
        }
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var request = _requestFactory.BuildRequest();
            var response = _client.Send(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request to {request.RequestUri} failed with status code {response.StatusCode}");
            }

            var rates = _parser.Parse(response.Content.ReadAsStream());
            var result = new List<ExchangeRate>();

            var targetCurrency = new Currency("CZK");
            result.AddRange(rates
                .Where(r => currencies.Select(c => c.Code).Contains(r.currency))
                .Select(r => new ExchangeRate(r, targetCurrency, r.rate)));
            
            return result;
        }
    }
}
