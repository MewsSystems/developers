namespace ExchangeRateUpdater.Infrastructure.Configuration;

public class ExchangeRateServiceOptions
{
    public const string SectionName = "CnbExchangeRateService";

    public string CnbApiBaseUrl { get; set; } = string.Empty;
    public string CacheKeyName { get; set; } = string.Empty;
    public int CacheExpirationMinutes { get; set; } = 60;
    public int PublicationHour { get; set; } = 14;
    public int PublicationMinute { get; set; } = 30;
    public int RetryCount { get; set; } = 3;
    public int RetryBaseDelaySeconds { get; set; } = 2;
}