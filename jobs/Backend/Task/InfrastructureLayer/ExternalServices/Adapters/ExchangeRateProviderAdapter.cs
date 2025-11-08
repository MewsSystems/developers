using Common.Interfaces;
using InfrastructureLayer.ExternalServices.Adapters.Models;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.ExternalServices.Adapters;

/// <summary>
/// Adapter that wraps an existing IExchangeRateProvider and provides domain-friendly methods.
/// Converts between ExchangeRateDTO (provider layer) and ProviderRate (domain layer).
/// </summary>
public class ExchangeRateProviderAdapter : IExchangeRateProviderAdapter
{
    private readonly IExchangeRateProvider _provider;
    private readonly ILogger<ExchangeRateProviderAdapter> _logger;

    public string ProviderCode { get; }

    public ExchangeRateProviderAdapter(
        IExchangeRateProvider provider,
        string providerCode,
        ILogger<ExchangeRateProviderAdapter> logger)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(providerCode))
            throw new ArgumentException("Provider code cannot be null or empty", nameof(providerCode));

        ProviderCode = providerCode;
    }

    public async Task<ProviderRateResponse> FetchLatestRatesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching latest rates for provider {ProviderCode}", ProviderCode);

            var ((statusCode, message), rates) = await _provider.GetExchangeRatesForToday();

            if (statusCode != 200 || rates == null || !rates.Any())
            {
                _logger.LogWarning(
                    "Provider {ProviderCode} returned status {StatusCode}: {Message}",
                    ProviderCode,
                    statusCode,
                    message);

                return ProviderRateResponse.Failure($"Status {statusCode}: {message}");
            }

            var providerRates = rates.Select(r => new ProviderRate
            {
                SourceCurrencyCode = r.BaseCurrencyCode,
                TargetCurrencyCode = r.TargetCurrencyCode,
                Rate = r.Rate,
                Multiplier = r.Multiplier,
                ValidDate = r.ValidDate
            }).ToList();

            _logger.LogInformation(
                "Successfully fetched {Count} rates for {ProviderCode} with ValidDate {ValidDate}",
                providerRates.Count,
                ProviderCode,
                rates.Max(x => x.ValidDate));

            return ProviderRateResponse.Success(rates.Max(x => x.ValidDate), providerRates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching latest rates for {ProviderCode}", ProviderCode);
            return ProviderRateResponse.Failure($"Exception: {ex.Message}");
        }
    }

    public async Task<ProviderRateResponse> FetchHistoricalRatesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching historical rates for provider {ProviderCode}", ProviderCode);

            var ((statusCode, message), rates) = await _provider.GetHistoryExchangeRates();

            if (statusCode != 200 || rates == null || !rates.Any())
            {
                _logger.LogWarning(
                    "Provider {ProviderCode} historical fetch returned status {StatusCode}: {Message}",
                    ProviderCode,
                    statusCode,
                    message);

                return ProviderRateResponse.Failure($"Status {statusCode}: {message}");
            }

            // Get most recent ValidDate
            var validDate = rates.Max(r => r.ValidDate);

            var providerRates = rates.Select(r => new ProviderRate
            {
                SourceCurrencyCode = r.BaseCurrencyCode,
                TargetCurrencyCode = r.TargetCurrencyCode,
                Rate = r.Rate,
                Multiplier = r.Multiplier,
                ValidDate = r.ValidDate
            }).ToList();

            _logger.LogInformation(
                "Successfully fetched {Count} historical rates for {ProviderCode} up to {ValidDate}",
                providerRates.Count,
                ProviderCode,
                validDate);

            return ProviderRateResponse.Success(validDate, providerRates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching historical rates for {ProviderCode}", ProviderCode);
            return ProviderRateResponse.Failure($"Exception: {ex.Message}");
        }
    }

    public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Basic health check: try to fetch latest rates
            var ((statusCode, _), rates) = await _provider.GetExchangeRatesForToday();
            return statusCode == 200 && rates != null && rates.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed for provider {ProviderCode}", ProviderCode);
            return false;
        }
    }
}
