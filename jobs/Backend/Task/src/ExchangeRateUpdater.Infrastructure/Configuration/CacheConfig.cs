namespace ExchangeRateUpdater.Infrastructure.Configuration;

public class CacheConfig
{
    public int DailyRatesAbsoluteExpirationInMinutes { get; init; } = 30;
    public int DailyRatesSlidingExpirationInMinutes { get; init; } = 10;
    public int MonthlyRatesAbsoluteExpirationInMinutes { get; init; } = 1440;
    public int MonthlyRatesSlidingExpirationInMinutes { get; init; } = 60;
} 