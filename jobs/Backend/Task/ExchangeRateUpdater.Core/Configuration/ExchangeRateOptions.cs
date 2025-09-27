namespace ExchangeRateUpdater.Core.Configuration;

public class ExchangeRateOptions
{
    public const string SectionName = "ExchangeRate";
    public int MaxRetryAttempts { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(2);
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(30);
    public bool EnableCaching { get; set; } = true;
}
