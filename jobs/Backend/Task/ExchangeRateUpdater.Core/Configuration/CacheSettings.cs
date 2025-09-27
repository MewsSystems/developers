namespace ExchangeRateUpdater.Core.Configuration;

public class CacheSettings
{
    public TimeSpan DefaultCacheExpiry { get; set; } = TimeSpan.FromHours(12);
    public int SizeLimit { get; set; } = 1000;
    public double CompactionPercentage { get; set; } = 0.25;
    public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(5);
}