using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private const int MAX_RETRIES = 3;
        private const int SECONDS_TO_RETRY = 2;

        public ExchangeRateProvider()
        {
            _httpClient = new HttpClient();
            // Define the retry policy specific for HttpResponseMessage
            _retryPolicy = Policy
                           .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                           .Or<HttpRequestException>()
                           .WaitAndRetryAsync(MAX_RETRIES, retryAttempt => TimeSpan.FromSeconds(SECONDS_TO_RETRY),
                               (outcome, timespan, retryCount, context) =>
                               {
                                   Log.Warning("Retrying due to {Reason}. Retry attempt: {RetryCount}",
                                       outcome.Result?.StatusCode.ToString() ?? outcome.Exception?.Message, retryCount);
                               });
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            const string CZECH_CODE = "CZK";
            DateTime currentDate = DateTime.Now;
            string url = "https://api.cnb.cz/cnbapi/exrates/daily?date=" + currentDate.ToString("yyyy-MM-dd") + "&lang=EN";
            try
            {
                Log.Debug("Sending request to {Url}", url);
                var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(errorContent);

                    Log.Error("API call failed with error: {ErrorCode}, Description: {Description}",
                        errorResponse.ErrorCode, errorResponse.Description);

                    return Enumerable.Empty<ExchangeRate>();
                }

                var content = await response.Content.ReadAsStringAsync();
                Log.Debug("Received response: {Response}", content);
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                var rates = apiResponse.Rates
                    .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
                    .Select(r => new ExchangeRate(new Currency(CZECH_CODE), new Currency(r.CurrencyCode), r.Rate));

                return rates;
            }
            catch (Exception e)
            {
                Log.Debug($"Error retrieving or processing data: {e.Message}");
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }

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
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }

    public class ApiErrorResponse
    {
        public string Description { get; set; }
        public string EndPoint { get; set; }
        public string ErrorCode { get; set; }
        public DateTime HappenedAt { get; set; }
        public string MessageId { get; set; }
    }
}
