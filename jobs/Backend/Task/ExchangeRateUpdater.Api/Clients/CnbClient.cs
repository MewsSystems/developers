using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRateUpdater.Api.Clients
{
    public interface ICnbClient
    {
        Task<CnbDailyExchangeRatesResponse> GetExchangeRatesAsync(CancellationToken cancellationToken);
    }

    public class CnbClient : ICnbClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public CnbClient(HttpClient httpClient, IOptions<SourceConfiguration> sourceConfiguration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _endpoint = sourceConfiguration?.Value?.DailyExchangeRatesEndpoint ?? throw new ArgumentNullException(nameof(sourceConfiguration));
        }

        public async Task<CnbDailyExchangeRatesResponse> GetExchangeRatesAsync(CancellationToken cancellationToken)
        {
            var requestUri = $"{_httpClient.BaseAddress}/{_endpoint}";
            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var payload = JsonSerializer.Deserialize<CnbDailyExchangeRatesResponse>(responseContent) ?? new CnbDailyExchangeRatesResponse();

            return payload;
        }
    }
}
