using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{

    public partial class ExchangeRateProvider : IExchangeRateProvider
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _apiUrl;
        private readonly Currency _baseCurrency;

        public ExchangeRateProvider(IExchangeRateProviderConfiguration config)
        {
            if (config == null) 
            { 
                throw new ArgumentNullException(nameof(config)); 
            }

            _apiUrl = config.Url;
            _baseCurrency = new Currency(config.BaseCurrency);
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var apiResponse = await HttpClient.GetFromJsonAsync<ApiResponse>(_apiUrl);

            var rates = new List<ExchangeRate>();
            var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            if (apiResponse?.Rates == null)
            { 
                return rates; 
            }

            foreach (var rate in apiResponse.Rates)
            {
                if (!currencyCodes.Contains(rate.CurrencyCode))
                {
                    continue;
                }

                var currency = new Currency(rate.CurrencyCode);
                rates.Add(new ExchangeRate(currency, _baseCurrency, rate.Rate / rate.Amount));
            }
            return rates;
        }
    }
}
