namespace ExchangeRateUpdater.ExchangeClients
{
    using ExchangeRateUpdater.Exceptions;
    using ExchangeRateUpdater.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CnbExchangeRateClient : IExchangeRateClient
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateProviderSettings _settings;
        private readonly ILogger<CnbExchangeRateClient> _logger;

        public CnbExchangeRateClient(HttpClient httpClient, IOptions<ExchangeRateProviderSettings> settings, ILogger<CnbExchangeRateClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutInSeconds);
        }

        public async Task<string> GetDailyRatesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(_settings.CnbUrl, cancellationToken);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while fetching daily rates from CNB.");
                throw new ExchangeRateUpdateException("Error fetching daily rates from CNB.", ex);
            }
        }
    }
}
