using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Example API response from https://api.cnb.cz/cnbapi/exrates/daily?date=2019-05-10
    /// 
    /// {
    ///     "rates": [
    ///        {
    ///            "validFor": "2019-05-10",
    ///            "order": 89,
    ///            "country": "Brazílie",
    ///            "currency": "real",
    ///            "amount": 1,
    ///            "currencyCode": "BRL",
    ///            "rate": 5.796
    ///     
    ///         },
    ///         { ... },
    ///         ...
    ///     ]
    /// }
    /// </summary>

    public class ApiResponse
    {
        public List<ApiExchangeRate> Rates { get; set; }
    }

    public class ApiExchangeRate
    {
        public string ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public long Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }

    public class ExchangeRateProvider
    {
        private readonly HttpClient _client;

        // Injecting client here for testing (mocking) purposes
        public ExchangeRateProvider(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var baseUrl = "https://api.cnb.cz/cnbapi/exrates/daily";
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string url = $"{baseUrl}?date={date}";
            Console.WriteLine(url);

            var parsed = await _client.GetFromJsonAsync<ApiResponse>(url);

            List<ExchangeRate> rates = new List<ExchangeRate>();

            foreach (ApiExchangeRate item in parsed.Rates)
            {
                var sourceCurrency = new Currency(item.CurrencyCode);
                var targetCurrency = new Currency("CZK");
                var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, item.Rate, item.Amount);

                if (currencies.Where((currency) => currency.Code == sourceCurrency.Code).Any())
                {
                    rates.Add(exchangeRate);
                }
                else
                {
                    //Console.WriteLine($"Skipping the exchange rate for {sourceCurrency}.");
                }
            }

            return rates;
        }
    }
}