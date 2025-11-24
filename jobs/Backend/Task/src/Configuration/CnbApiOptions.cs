namespace ExchangeRateUpdater;

public class CnbApiOptions
{
    public const string SectionName = "CnbApi";

    public string BaseUrl { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; }
    public int RetryCount { get; set; }
    public int CircuitBreakerFailureThreshold { get; set; }
    public int CircuitBreakerDurationSeconds { get; set; }
}
