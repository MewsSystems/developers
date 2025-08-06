namespace ExchangeRateUpdater.Infrastructure.Services;

public class CacheConfiguration
{
    public TimeSpan DailyRatesExpiration { get; set; } = TimeSpan.FromHours(6);
    public TimeSpan MonthlyRatesExpiration { get; set; } = TimeSpan.FromDays(1);
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
} 