using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Exceptions;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Runtime.ConstrainedExecution;

namespace ExchangeRateUpdater.Clients
{
    /// <summary>
    /// Czech National Bank specific implementation of exchange rate API client.
    /// Fetches data from CNB's daily exchange rate fixing endpoint.
    /// </summary>
    public class CnbApiClient : IExchangeRateApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateApiSettings _settings;

        public CnbApiClient(HttpClient httpClient, IOptions<ExchangeRateApiSettings> settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        }

        public async Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(_settings.DailyRatesUrl, cancellationToken);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            //Consistent error handling regardless of which client implementation is used
            catch (HttpRequestException ex)
            {
                throw new ExchangeRateException(
                    "Failed to retrieve exchange rates from CNB API.",
                    ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new ExchangeRateException(
                    "Request to CNB API timed out.",
                    ex);
            }
        }
    }
}