using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExchangeRateProvider.Domain.Entities;
using Microsoft.Extensions.Logging;
using ExchangeRateProvider.Domain.Interfaces;

namespace ExchangeRateProvider.Infrastructure
{
    /// <summary>
    /// CNB exchange rate provider using the official JSON API.
    /// </summary>
    public sealed class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CnbExchangeRateProvider> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Gets the name of this provider.
        /// </summary>
        public string Name => "CNB";

        /// <summary>
        /// Gets the priority of this provider (higher values = higher priority).
        /// CNB is a reliable official source, so high priority.
        /// </summary>
        public int Priority => 100;

        /// <summary>
        /// Determines whether this provider can handle the specified currencies.
        /// CNB provides rates for many currencies, so we can handle most requests.
        /// </summary>
        public bool CanHandle(IEnumerable<Currency> currencies)
        {
            // CNB provides rates for many currencies, but we should validate the codes
            var requestedCodes = currencies
                ?.Select(c => c.Code?.ToUpperInvariant())
                .Where(code => !string.IsNullOrEmpty(code))
                .ToList() ?? [];

            if (!requestedCodes.Any())
            {
                return false;
            }

            return true;
        }

        public CnbExchangeRateProvider(IHttpClientFactory httpClientFactory, ILogger<CnbExchangeRateProvider> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> requestedCurrencies, 
            CancellationToken cancellationToken = default)
        {
            var requestedCodes = requestedCurrencies
                ?.Select(c => c.Code?.ToUpperInvariant())
                .Where(code => !string.IsNullOrEmpty(code))
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

            if (!requestedCodes.Any())
            {
                _logger.LogWarning("No valid currencies requested");
                return [];
            }

            try
            {
                _logger.LogDebug("Fetching CNB rates for {Count} currencies: {Currencies}",
                    requestedCodes.Count, string.Join(", ", requestedCodes));

                using var httpClient = _httpClientFactory.CreateClient("CnbExchangeRateProvider");
                var jsonResponse = await httpClient.GetStringAsync("cnbapi/exrates/daily", cancellationToken);
                var cnbData = JsonSerializer.Deserialize<CnbApiResponse>(jsonResponse, _jsonOptions);

                if (cnbData?.Rates == null)
                {
                    _logger.LogError("CNB API returned invalid response structure");
                    return [];
                }

                var result = new List<ExchangeRate>();
                var czkCurrency = new Currency("CZK");

                foreach (var rate in cnbData.Rates)
                {
                    if (string.IsNullOrEmpty(rate.CurrencyCode) || 
                        !requestedCodes.Contains(rate.CurrencyCode))
                    {
                        continue;
                    }

                    if (rate.Amount <= 0 || rate.Rate <= 0)
                    {
                        _logger.LogWarning("Invalid rate data for {Currency}: Amount={Amount}, Rate={Rate}", 
                            rate.CurrencyCode, rate.Amount, rate.Rate);
                        continue;
                    }

                    // Return EXACTLY what CNB provides: X units of foreign currency = Y CZK
                    // No calculations - use CNB's exact values
                    var exchangeRate = new ExchangeRate(
                        sourceCurrency: new Currency(rate.CurrencyCode.ToUpperInvariant()),
                        targetCurrency: czkCurrency,
                        value: rate.Rate        // Exact CNB rate value
                    );

                    result.Add(exchangeRate);
                }

                _logger.LogInformation("Successfully retrieved {Count} exchange rates from CNB (Date: {Date})", 
                    result.Count, cnbData.Date);

                return result.AsReadOnly();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error fetching CNB rates");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse CNB JSON response");
                throw new InvalidOperationException("CNB API returned invalid JSON format", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching CNB exchange rates");
                throw;
            }
        }
    }

    /// <summary>
    /// CNB API JSON response structure.
    /// </summary>
    internal sealed class CnbApiResponse
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("rates")]
        public List<CnbRate>? Rates { get; set; }
    }

    /// <summary>
    /// currency rate from CNB API.
    /// </summary>
    internal sealed class CnbRate
    {
        [JsonPropertyName("validFor")]
        public string? ValidFor { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}