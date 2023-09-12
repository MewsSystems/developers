using ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.Logging;
using Polly;
using RestSharp;

namespace ExchangeRateUpdater.Infrastructure.Clients
{
    internal class CzechNationalBankApiClient : RestClientBase, ICzechNationalBankApiClient
    {
        private const string ExchangeRateDailyUrl = "cnbapi/exrates/daily";
        private const int RetryCount = 3;

        private readonly IAsyncPolicy<RestResponse<CzechNationalBankExchangeRatesResponse>> _retryPolicy;
        private readonly ILogger<CzechNationalBankApiClient> _logger;

        public CzechNationalBankApiClient(HttpClient httpClient, IMonitorProvider monitorProvider, ILogger<CzechNationalBankApiClient> logger)
            : base(httpClient, monitorProvider)
        {
            _logger = logger;

            _retryPolicy = Policy.HandleResult<RestResponse<CzechNationalBankExchangeRatesResponse>>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(RetryCount, a => TimeSpan.FromSeconds(Math.Pow(2, a)));
        }

        public async Task<CzechNationalBankExchangeRatesResponse> GetExchangeRatesAsync(DateTime? date = null, string lang = "EN")
        {
            _logger.LogInformation("CzechNationalBankApiClient.GetExchangeRatesAsync: Start getting exchange rates from CzechNationalBankApi");

            var request = CreateRequest(date ?? DateTime.Now, lang);

            var response = await _retryPolicy.ExecuteAsync(async () => 
                    await ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(request, "GetExchangeRatesAsync_Exec_Time")
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError(response.ErrorException, "CzechNationalBankApiClient.GetExchangeRatesAsync: Failed to get exchange rates from CzechNationalBankApi");
                throw new ApplicationException("Unable to get exchange rates from CzechNationalBankApi", response.ErrorException);
            }

            return response.Data ?? new CzechNationalBankExchangeRatesResponse();
        }

        private RestRequest CreateRequest(DateTime date, string lang)
        {
            var request = new RestRequest(ExchangeRateDailyUrl);
            request.AddParameter("lang", lang);
            request.AddParameter("date", date.ToString("yyyy-MM-dd"));
            return request;
        }

        protected override string ClientName => "CzechNationalBankApiClient";
    }
}
