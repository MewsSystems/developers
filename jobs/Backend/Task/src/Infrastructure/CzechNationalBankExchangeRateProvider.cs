using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
    public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
    {
        private class OkResponse
        {
            public class OkRexponseExchangeRate
            {
                public int Amount { get; set; }
                public string CurrencyCode { get; set; }
                public decimal Rate { get; set; }
            }

            public IEnumerable<OkRexponseExchangeRate> Rates { get; set; }
        }

        private class ErrorResponse
        {
            public string Description { get; set; }
        }

        // This is a magic string which should not be in code
        // Correct way will be to set up appsettings.json and pass IConfiguration
        // into this class constructor to get the configure value
        private const string EXCHANGE_RATES_DAILY_URL = "https://api.cnb.cz/cnbapi/exrates/daily";
        private Dictionary<string, string> Params = new Dictionary<string, string> { { "lang", "EN" } };

        private readonly IRestClient _restClient;
        private readonly ILogger _logger;

        public CzechNationalBankExchangeRateProvider(IRestClient restClient, ILogger<CzechNationalBankExchangeRateProvider> logger)
        {
            _restClient ??= restClient ?? throw new ArgumentNullException(nameof(restClient));
            _logger ??= logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var request = new RestRequest(EXCHANGE_RATES_DAILY_URL);
            Params.ToList().ForEach(p => request.AddParameter(p.Key, p.Value));

            _logger.LogInformation($"Fetching exchange rates from Czech National Bank API: {request.Method} {request.Resource}...");

            var response = await _restClient.ExecuteAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                var errorMessage = $"Request: {request.Method} {response.ResponseUri} failed with status code: {response.StatusCode} message: {errorResponse.Description}";
                
                _logger.LogError(errorMessage);
                throw new ApplicationException(errorMessage);
            }

            _logger.LogInformation($"Czech National Bank API successful response: {JsonConvert.DeserializeObject<object>(response.Content)}");

            if (string.IsNullOrWhiteSpace(response.Content))
                return new ExchangeRate[0];

            var responseExchangeRates = JsonConvert.DeserializeObject<OkResponse>(response.Content).Rates;

            currencies = currencies.Where(c => c != null && !string.IsNullOrWhiteSpace(c.Code));
            responseExchangeRates = responseExchangeRates.Where(r => currencies.Any(c => c.Code == r.CurrencyCode));

            var exchangeRates = new List<ExchangeRate>();
            
            foreach (var currency in currencies)
            {
                var responseExchangeRate = responseExchangeRates.FirstOrDefault(r => r.CurrencyCode == currency.Code);
                if (responseExchangeRate != null)
                    exchangeRates.Add(new ExchangeRate(currency, Currency.DEFAULT_CURRENCY, Math.Round(responseExchangeRate.Rate / responseExchangeRate.Amount, 2)));
            }

            return exchangeRates;
        }
    }
}
