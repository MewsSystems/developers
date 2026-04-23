using ExchangeRateUpdater.Infrastructure.CNB.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<ExRateDailyResponse> GetExDailyRates(CancellationToken cancellationToken)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_baseUrl}/exrates/daily?lang=EN");

            HttpClient httpClient = _httpClientFactory.CreateClient();
            try
            {
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
                httpResponseMessage.EnsureSuccessStatusCode();    // Throw if not a success code.
                using var contentStream =
                   await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = await JsonSerializer.DeserializeAsync
                <ExRateDailyResponse>(contentStream, options, cancellationToken);

                _logger.LogDebug($"Received rates for {string.Join(",", result.Rates.Select(r => r.CurrencyCode))}");
                return result;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error downloading exchange rates");
                return null;
            }
        }
    }
}
