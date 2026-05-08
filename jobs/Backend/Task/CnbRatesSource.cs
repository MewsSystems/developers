using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Fetches exchange rates from the Czech National Bank daily fixing endpoint.
    /// </summary>
    public class CnbRatesSource : IExchangeRatesSource
    {
        private const string CnbDailyRatesPath =
            "/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(10) };

        private readonly string _ratesUrl;
        private readonly ILogger<CnbRatesSource> _logger;

        public CnbRatesSource(string baseUrl, ILogger<CnbRatesSource> logger)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));

            _ratesUrl = baseUrl.TrimEnd('/') + CnbDailyRatesPath;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetLatestRatesContent()
        {
            _logger.LogDebug("Fetching exchange rates from CNB: {Url}", _ratesUrl);

            try
            {
                return HttpClient.GetStringAsync(_ratesUrl).GetAwaiter().GetResult();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "HTTP request failed for {Url}", _ratesUrl);
                throw new InvalidOperationException(
                    $"Failed to fetch exchange rates from {_ratesUrl}: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "Request timed out for {Url}", _ratesUrl);
                throw new InvalidOperationException(
                    $"Request timed out while fetching exchange rates from {_ratesUrl}", ex);
            }
        }
    }
}
