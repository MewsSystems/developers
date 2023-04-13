using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ExchangeRateUpdater.Services
{
    public class CzechExchangeRateFetcher : IExchangeRateFetcher
    {
        private readonly CNBSettings _cnbSettings;
        private readonly HttpClient _httpClient;

        public CzechExchangeRateFetcher(HttpClient httpClient, IOptions<CNBSettings> settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cnbSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<string> FetchDailyExchangeRateData(DateOnly? date)
        {
            return await FetchExchangeRateDataAsync(BuildDailyUrl(date));
        }

        public async Task<string> FetchMonthlyExchangeRateData(DateOnly? date)
        {
            return await FetchExchangeRateDataAsync(BuildMonthlyUrl(date));
        }

        private async Task<string> FetchExchangeRateDataAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
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
