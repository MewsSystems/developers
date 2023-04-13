using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateFetcher : IExchangeRateFetcher
    {
        private readonly string _cnbUrl;
        private readonly HttpClient _httpClient;
        private readonly IClock _clock;

        public ExchangeRateFetcher(HttpClient httpClient, IOptions<CNBSettings> settings, IClock clock)
        {
            _httpClient = httpClient;
            _cnbUrl = settings.Value.Url;
            _clock = clock;
        }

        public async Task<string> FetchExchangeRateData()
        {
            var response = await _httpClient.GetAsync(BuildUrl());
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private string BuildUrl()
        {
            var requestUri = new UriBuilder(_cnbUrl);
            var query = HttpUtility.ParseQueryString(requestUri.Query);
            query["date"] = _clock.Today.ToString(Constants.DateFormat);
            requestUri.Query = query.ToString();

            return requestUri.ToString();
        }
    }
}
