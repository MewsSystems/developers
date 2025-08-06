using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.Tests.ExchangeRates.CzechNationalBank.Models;

public class CzechNationalBankExchangeRateConfigTests
{
    [Fact]
    public void CzechNationalBankExchangeRateConfig_WithValidData_ShouldCreateSuccessfully()
    {
        var config = new CzechNationalBankExchangeRateConfig
        {
            Cache = new CacheConfig
            {
                DailyRatesAbsoluteExpirationInMinutes = 30,
                DailyRatesSlidingExpirationInMinutes = 10,
                MonthlyRatesAbsoluteExpirationInMinutes = 1440,
                MonthlyRatesSlidingExpirationInMinutes = 60
            }
        };

        Assert.NotNull(config);
        Assert.NotNull(config.Cache);
        Assert.Equal(30, config.Cache.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(10, config.Cache.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(1440, config.Cache.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(60, config.Cache.MonthlyRatesSlidingExpirationInMinutes);
    }

    [Fact]
    public void CacheConfig_WithValidData_ShouldCreateSuccessfully()
    {
        var cacheConfig = new CacheConfig
        {
            DailyRatesAbsoluteExpirationInMinutes = 60,
            DailyRatesSlidingExpirationInMinutes = 15,
            MonthlyRatesAbsoluteExpirationInMinutes = 2880,
            MonthlyRatesSlidingExpirationInMinutes = 120
        };

        Assert.Equal(60, cacheConfig.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(15, cacheConfig.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(2880, cacheConfig.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(120, cacheConfig.MonthlyRatesSlidingExpirationInMinutes);
    }

    [Fact]
    public void CacheConfig_WithZeroValues_ShouldCreateSuccessfully()
    {
        var cacheConfig = new CacheConfig
        {
            DailyRatesAbsoluteExpirationInMinutes = 0,
            DailyRatesSlidingExpirationInMinutes = 0,
            MonthlyRatesAbsoluteExpirationInMinutes = 0,
            MonthlyRatesSlidingExpirationInMinutes = 0
        };

        Assert.Equal(0, cacheConfig.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(0, cacheConfig.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(0, cacheConfig.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(0, cacheConfig.MonthlyRatesSlidingExpirationInMinutes);
    }

    [Fact]
    public void CacheConfig_WithNegativeValues_ShouldCreateSuccessfully()
    {
        var cacheConfig = new CacheConfig
        {
            DailyRatesAbsoluteExpirationInMinutes = -1,
            DailyRatesSlidingExpirationInMinutes = -5,
            MonthlyRatesAbsoluteExpirationInMinutes = -10,
            MonthlyRatesSlidingExpirationInMinutes = -20
        };

        Assert.Equal(-1, cacheConfig.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(-5, cacheConfig.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(-10, cacheConfig.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(-20, cacheConfig.MonthlyRatesSlidingExpirationInMinutes);
    }
} 