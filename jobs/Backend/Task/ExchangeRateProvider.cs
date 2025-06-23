using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly string CnbUrl = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";
        private static readonly Currency Czk = new Currency("CZK");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await HttpClient.GetStringAsync(CnbUrl);
            var rates = new List<ExchangeRate>();
            var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            var cnbResponse = JsonConvert.DeserializeObject<CnbApiResponse>(response);
            if (cnbResponse?.Rates == null)
                return rates;

            foreach (var rate in cnbResponse.Rates)
            {
                if (!currencyCodes.Contains(rate.CurrencyCode))
                    continue;
                var currency = new Currency(rate.CurrencyCode);
                rates.Add(new ExchangeRate(currency, Czk, rate.Rate / rate.Amount));
            }
            return rates;
        }

        private class CnbApiResponse
        {
            [JsonProperty("rates")]
            public List<CnbApiRate> Rates { get; set; }
        }

        private class CnbApiRate
        {
            [JsonProperty("currencyCode")]
            public string CurrencyCode { get; set; }
            [JsonProperty("amount")]
            public int Amount { get; set; }
            [JsonProperty("rate")]
            public decimal Rate { get; set; }
        }
    }
}
