using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Models;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ExchangeRateUpdater.Data
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly Currency sourceCurrency;
        private readonly string baseAddress;
        private readonly string endpoint;
        private readonly RetryConfiguration retryConfiguration;

        private HttpClient cnbClient;

        public CnbExchangeRateProvider(IHttpClientFactory httpClientFactory, ILogger<CnbExchangeRateProvider> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            cnbClient = httpClientFactory.CreateClient(nameof(CnbExchangeRateProvider));
            var cnbConfig = config.GetSection("CnbConfiguration");
            sourceCurrency = new Currency(cnbConfig.GetValue<string>("DefaultCurrency"));
            baseAddress = cnbConfig.GetValue<string>("BaseAddress");
            endpoint = cnbConfig.GetValue<string>("DailyRatesEndpoint");
            retryConfiguration = cnbConfig.GetSection("RetryConfig").Get<RetryConfiguration>();
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var clientAddress = $"{baseAddress}/{endpoint}?lang=EN";
            _logger.LogInformation($"{nameof(CnbExchangeRateProvider)}: Sending request to {clientAddress}");
            var response = await ExecuteCallWithRetryAsync(clientAddress);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var cnbResponse = JsonSerializer.Deserialize<CnbDailyRatesResponse>(responseContent) ?? new CnbDailyRatesResponse();
            _logger.LogInformation($"{nameof(CnbExchangeRateProvider)}: Response received from {clientAddress}, message: {cnbResponse}");
            return TranslateCnbResposne(cnbResponse, currencies);
        }

        private async Task<HttpResponseMessage> ExecuteCallWithRetryAsync(string clientAddress)
        {
            var timeoutPerTry = Policy.TimeoutAsync(retryConfiguration.TimeoutSeconds);
            return await Policy.HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode)
                .WaitAndRetryAsync(retryConfiguration.Retries, _ => retryConfiguration.WaitSeconds, OnTryFailure)
                .WrapAsync(timeoutPerTry)
                .ExecuteAsync(() => cnbClient.GetAsync($"{clientAddress}"));
        }

        private IEnumerable<ExchangeRate> TranslateCnbResposne(CnbDailyRatesResponse cnbResponse, IEnumerable<Currency> currencies)
        {
            return cnbResponse.Rates
                .Where(rate => currencies.Any(curr => curr.Code == rate.CurrencyCode))
                .Select(rate => new ExchangeRate(sourceCurrency, new Currency(rate.CurrencyCode), Math.Round(rate.Rate, 2)));
        }

        private void OnTryFailure(DelegateResult<HttpResponseMessage> response, TimeSpan timeToNext, int retryCount, Context context)
        {
            var resultMessage = response.Result.Content.ReadAsStringAsync().Result;
            _logger.LogError($"Failed to retreive exchange rates. Status code: {response.Result.StatusCode}," +
                $"base address: {baseAddress}, endpoint: {endpoint}, retry count: {retryCount}, message: {resultMessage}");
        }
    }
}
