namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Configuration;

/// <summary>
/// Configuration options for the Czech National Bank exchange rate provider.
/// </summary>
internal record ProviderOptions
{
    public static string ConfigurationSectionName => "CnbProvider";
    public required string BaseUrl { get; set; } = string.Empty;
    public required string DailyRatesPath { get; set; } = string.Empty;
    public required int TimeoutSeconds { get; set; } = 10;
    public required int DurationOfCircuitBreakSeconds { get; set; } = 30;
    public required int RetryCount { get; set; } = 3;
    public required string UserAgent { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            throw new InvalidOperationException("BaseUrl is required");
        }

        if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
        {
            throw new InvalidOperationException("BaseUrl must be a valid URL");
        }

        if (string.IsNullOrWhiteSpace(DailyRatesPath))
        {
            throw new InvalidOperationException($"{nameof(DailyRatesPath)} is required");
        }

        if (TimeoutSeconds <= 0)
        {
            throw new InvalidOperationException($"{nameof(TimeoutSeconds)} must be positive");
        }

        if (DurationOfCircuitBreakSeconds <= 0)
        {
            throw new InvalidOperationException($"{nameof(DurationOfCircuitBreakSeconds)} must be positive");
        }

        if (RetryCount < 0)
        {
            throw new InvalidOperationException($"{nameof(RetryCount)} cannot be negative");
        }
    }
}