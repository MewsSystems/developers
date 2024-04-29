using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string CZECH_KORUNA_CODE = "CZK";
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(HttpClient httpClient, ILogger<ExchangeRateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(currencies is null || !currencies.Any()) {
                return Enumerable.Empty<ExchangeRate>();
            }

            var response = _httpClient.GetFromJsonAsync<ExchangeRateResponsePayload>("https://api.cnb.cz/cnbapi/exrates/daily?lang=EN").Result;

            if(response is null || response.Rates is null || !response.Rates.Any())
            {
                _logger.LogWarning("Exchange rate response contained no rates");

                return Enumerable.Empty<ExchangeRate>();
            }

            var returnRates = new List<ExchangeRate>();
            var payloadRates = response.Rates;
            foreach(var currency in currencies) 
            {
                var rate = payloadRates.FirstOrDefault(r => r.CurrencyCode == currency.Code);
                if(rate is not null){
                    returnRates.Add(new ExchangeRate(new Currency(CZECH_KORUNA_CODE), new Currency(rate.CurrencyCode), rate.Rate));
                }
            }

            return returnRates.ToArray();
        }

        protected record ExchangeRateResponsePayload(ExchangeRateResponse[] Rates);

        protected record ExchangeRateResponse(string CurrencyCode, decimal Rate, int Amount);
    }
}

