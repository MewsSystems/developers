using System;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.HttpClients
{
    public class OtherCurrencyExchangeRateFetcher : IExhangeRateFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly ILogger<OtherCurrencyExchangeRateFetcher> _logger;

        public OtherCurrencyExchangeRateFetcher(
            HttpClient httpClient,
            IOptions<CzechBankSettings> options,
            ILogger<OtherCurrencyExchangeRateFetcher> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _url = options.Value.OtherCurrencyRatesUrl ?? throw new ArgumentNullException(nameof(options.Value.OtherCurrencyRatesUrl));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> FetchAsync()
        {
            _logger.LogInformation("Fetching other currency exchange rates from {Url}", _url);

            try
            {
                var start = DateTimeOffset.UtcNow;

                var response = await _httpClient.GetAsync(_url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var duration = DateTimeOffset.UtcNow - start;
                _logger.LogInformation("Fetched other currency exchange rates in {Duration} ms", duration.TotalMilliseconds);

                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching other currency exchange rates from {Url}", _url);
                throw;
            }
        }
    }
}
