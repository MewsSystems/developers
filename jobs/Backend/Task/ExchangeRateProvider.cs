using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NLog;

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
        // Parsing all fields from the API
        // The types are derived from CNB API docs - https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates/dailyUsingGET_1
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
        private readonly Logger _logger;
        private const string BaseUrl = "https://api.cnb.cz/cnbapi/exrates/daily";

        // Injecting client here for testing (mocking) purposes
        public ExchangeRateProvider(HttpClient client, Logger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string url = $"{BaseUrl}?date={date}";

            _logger.Info($"Getting the source data from {url}.");

            var parsed = await _client.GetFromJsonAsync<ApiResponse>(url);
            var rates = new List<ExchangeRate>();

            // Safer to convert to a list first to avoid possible multiple iterations over unknown IEnumerable 
            // Possible drawback for a very large currency lists
            var currencyList = currencies.ToList();

            foreach (ApiExchangeRate item in parsed.Rates)
            {
                var sourceCurrency = new Currency(item.CurrencyCode);
                var targetCurrency = new Currency("CZK");
                var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, item.Rate, item.Amount);

                if (currencyList.Any(currency => currency.Code == sourceCurrency.Code))
                {
                    rates.Add(exchangeRate);
                }
                else
                {
                    _logger.Info($"Skipping the exchange rate for {sourceCurrency}.");
                }
            }

            return rates;
        }
    }
}