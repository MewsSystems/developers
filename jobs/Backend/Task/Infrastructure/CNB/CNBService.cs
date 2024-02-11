using ExchangeRateUpdater.Infrastructure.CNB.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Serilog;

namespace ExchangeRateUpdater.Infrastructure.CNB
{
    internal class CNBService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;

        private readonly string _baseUrl = "https://api.cnb.cz/cnbapi";

        public CNBService(IHttpClientFactory httpClientFactoty, ILogger logger)
        {
            _httpClientFactory = httpClientFactoty;
            _logger = logger;
        }

        public async Task<ExRateDailyResponse> GetExDailyRates()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_baseUrl}/exrates/daily?lang=EN");

            HttpClient httpClient = _httpClientFactory.CreateClient();
            try
            {
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();    // Throw if not a success code.
                using var contentStream =
                   await httpResponseMessage.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = await JsonSerializer.DeserializeAsync
                <ExRateDailyResponse>(contentStream, options);

                _logger.Information($"Received rates for {string.Join(",", result.Rates.Select(r => r.CurrencyCode))}");
                return result;
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e, "Error downloading exchange rates");
                return null;
            }
        }
    }
}
