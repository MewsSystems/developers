using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.ExternalVendors.CzechNationalBank
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;
        private readonly CzechNationalBankSettings _settings = new CzechNationalBankSettings();
        
        private HttpClient _client = new HttpClient();
        private static readonly ConcurrentDictionary<string, Rate> RateStorage = new ConcurrentDictionary<string, Rate>();
        
        public CzechNationalBankExchangeRateProvider(ILogger<CzechNationalBankExchangeRateProvider> logger, IConfiguration configuration)
        {
            _logger = logger;
            configuration.GetSection(CzechNationalBankSettings.Position).Bind(_settings);
        }
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            _logger.LogInformation("Retrieving exchange rates for { currencies }", currencies);
            var rates = new List<ExchangeRate>();

            var uri = new Uri(this._settings.BASE_URL, "/cnbapi/exrates/daily?date=2019-05-17&lang=EN");
            var exchangeRates = await _client.GetFromJsonAsync<ExchangeRateDto>(uri);

            foreach (Rate rate in exchangeRates.Rates)
            {
                RateStorage.AddOrUpdate(rate.CurrencyCode, rate, (s, rate1) => rate);
            }

            foreach (Currency currency in currencies)
            {
                if (RateStorage.TryGetValue(currency.Code, out var requestedCurrencyRate))
                {
                    rates.Add(new ExchangeRate(currency, new Currency("CZK"), requestedCurrencyRate.RateRate / requestedCurrencyRate.Amount));
                }
            }

            return rates;
        }
    }
}
