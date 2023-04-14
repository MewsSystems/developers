using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ExchangeRateUpdater.Services
{
    // This class is responsible for fetching exchange rate data
    // from the Czech National Bank's website.
    public class CnbExchangeRateFetcher : IExchangeRateFetcher
    {
        private readonly CnbSettings _cnbSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CnbExchangeRateFetcher(IOptions<CnbSettings> settings, IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
            _cnbSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<string> FetchDailyExchangeRateData(DateOnly? date, CancellationToken cancellationToken = default)
        {
            return await FetchExchangeRateDataAsync(BuildDailyUrl(date), cancellationToken);
        }

        public async Task<string> FetchMonthlyExchangeRateData(DateOnly? date, CancellationToken cancellationToken = default)
        {
            return await FetchExchangeRateDataAsync(BuildMonthlyUrl(date), cancellationToken);
        }

        private async Task<string> FetchExchangeRateDataAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(Constants.CnbHttpClientKey);
                var response = await client.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Failed to fetch exchange rate data.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while fetching exchange rate data.", ex);
            }
        }

        private string BuildDailyUrl(DateOnly? date)
        {
            if (!date.HasValue)
                return _cnbSettings.DailyUrl;

            return BuildUrl(_cnbSettings.DailyUrl, new Dictionary<string, string>
            {
                ["date"] = date.Value.ToString(Constants.DateFormat)
            });
        }

        private string BuildMonthlyUrl(DateOnly? date)
        {
            if (!date.HasValue)
                return _cnbSettings.MonthlyUrl;

            return BuildUrl(_cnbSettings.MonthlyUrl, new Dictionary<string, string>
            {
                ["month"] = date.Value.AddMonths(-1).Month.ToString(),
                ["year"] = date.Value.Year.ToString()
            });
        }

        private string BuildUrl(string baseUrl, IDictionary<string, string> queryParameters)
        {
            var requestUri = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(requestUri.Query);

            foreach (var kvp in queryParameters)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    query[kvp.Key] = kvp.Value;
                }
            }

            requestUri.Query = query.ToString();
            return requestUri.ToString();
        }
    }
}
