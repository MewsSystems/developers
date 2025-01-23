namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Options;

public class ExchangeRateProviderOptions
{
    public required string BaseUrl { get; set; }
    public int RetryAttemptsPerCall { get; set; }
    public TimeSpan CircuitBreakDuration { get; set; }
    public TimeSpan RetryInterval { get; set; }
    public TimeSpan Timeout { get; set; }
}