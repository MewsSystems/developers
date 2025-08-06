namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

public class CzechNationalBankExchangeRateConfig
{
    public CacheConfig Cache { get; set; }
    public string BaseUrl { get; set; }
    public int TimeoutSeconds { get; set; }
}

public class CacheConfig
{
    public int MonthlyRatesSlidingExpirationInMinutes { get; set; } = 1440;
    public int MonthlyRatesAbsoluteExpirationInMinutes { get; set; } = 30;
    public int DailyRatesSlidingExpirationInMinutes { get; set; } = 30;
    public int DailyRatesAbsoluteExpirationInMinutes { get; set; } = 1440;
}