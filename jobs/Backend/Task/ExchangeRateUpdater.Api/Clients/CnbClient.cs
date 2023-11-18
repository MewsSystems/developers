using ExchangeRateUpdater.Api.Models;
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

        public CnbClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<CnbDailyExchangeRatesResponse> GetExchangeRatesAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var payload = JsonSerializer.Deserialize<CnbDailyExchangeRatesResponse>(responseContent) ?? new CnbDailyExchangeRatesResponse();

            return payload;
        }
    }
}
