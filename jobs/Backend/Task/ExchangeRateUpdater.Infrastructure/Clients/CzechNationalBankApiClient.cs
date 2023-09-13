using ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace ExchangeRateUpdater.Infrastructure.Clients
{
    internal class CzechNationalBankApiClient : RestClientBase, ICzechNationalBankApiClient
    {
        private const string ExchangeRateDailyUrl = "cnbapi/exrates/daily";
        private readonly ILogger<CzechNationalBankApiClient> _logger;

        public CzechNationalBankApiClient(HttpClient httpClient, IMonitorProvider monitorProvider, ILogger<CzechNationalBankApiClient> logger)
            : base(httpClient, monitorProvider)
        {
            _logger = logger;
        }

        protected override string ClientName => "CzechNationalBankApiClient";

        public async Task<CzechNationalBankExchangeRatesResponse?> GetExchangeRatesAsync(DateTime? date = null, string lang = "EN")
        {
            _logger.LogInformation("CzechNationalBankApiClient.GetExchangeRatesAsync: Start getting exchange rates from CzechNationalBankApi");

            var request = CreateRequest(date ?? DateTime.Now, lang);

            var response = await ExecuteGetAsync<CzechNationalBankExchangeRatesResponse>(request, "GetExchangeRatesAsync_Exec_Time")
                    .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.ErrorException, "CzechNationalBankApiClient.GetExchangeRatesAsync: Failed to get exchange rates from CzechNationalBankApi");
                throw new ApplicationException("Unable to get exchange rates from CzechNationalBankApi", response.ErrorException);
            }

            return response.Data;
        }

        private RestRequest CreateRequest(DateTime date, string lang)
        {
            var request = new RestRequest(ExchangeRateDailyUrl);
            request.AddParameter("lang", lang);
            request.AddParameter("date", date.ToString("yyyy-MM-dd"));
            return request;
        }
    }
}
