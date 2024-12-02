namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Options;

public class ExchangeRateProviderCachingOptions
{
    public bool Enabled { get; set; }
    public TimeSpan SlidingCacheDuration { get; set; }
    public TimeSpan MaxCacheDuration { get; set; }
}