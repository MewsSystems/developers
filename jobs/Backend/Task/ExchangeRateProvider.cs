using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeRateUpdater
{
    public class RateProviderConfiguration
    {
        public string Url { get; set; }
        public string BaseCurrency { get; set; }
    }

    public class ExchangeRateProvider
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _cnbUrl;
        private readonly Currency _czk;

        public static RateProviderConfiguration GetConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var url = configuration["ApiConfiguration:Url"];
            var baseCurrency = configuration["ApiConfiguration:BaseCurrency"];

            if (string.IsNullOrWhiteSpace(url))
                throw new Exception("ApiConfiguration:Url is not set in appsettings.json");

            if (string.IsNullOrWhiteSpace(baseCurrency))
                throw new Exception("ApiConfiguration:BaseCurrency is not set in appsettings.json");
            
            return new RateProviderConfiguration { Url = url, BaseCurrency = baseCurrency };
        }

        public ExchangeRateProvider(RateProviderConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            _cnbUrl = config.Url;
            _czk = new Currency(config.BaseCurrency);
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await HttpClient.GetStringAsync(_cnbUrl);
            var rates = new List<ExchangeRate>();
            var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            var cnbResponse = JsonConvert.DeserializeObject<Response>(response);
            if (cnbResponse?.Rates == null)
                return rates;

            foreach (var rate in cnbResponse.Rates)
            {
                if (!currencyCodes.Contains(rate.CurrencyCode))
                    continue;
                var currency = new Currency(rate.CurrencyCode);
                rates.Add(new ExchangeRate(currency, _czk, rate.ExchangeRateValue / rate.Amount));
            }
            return rates;
        }

        private class Response
        {
            [JsonProperty("rates")]
            public List<Rate> Rates { get; set; }
        }

        private class Rate
        {
            [JsonProperty("currencyCode")]
            public string CurrencyCode { get; set; }
            [JsonProperty("amount")]
            public int Amount { get; set; }
            [JsonProperty("rate")]
            public decimal ExchangeRateValue { get; set; }
        }
    }
}
