using ExchangeRateUpdater.Infrastructure.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.Tests;

public class ExchangeRateProvidersConfigTests
{
    [Fact]
    public void ExchangeRateProvidersConfig_WithValidData_ShouldCreateSuccessfully()
    {
        var czechConfig = new CzechNationalBankExchangeRateConfig
        {
            Cache = new CacheConfig
            {
                DailyRatesAbsoluteExpirationInMinutes = 30,
                DailyRatesSlidingExpirationInMinutes = 10,
                MonthlyRatesAbsoluteExpirationInMinutes = 1440,
                MonthlyRatesSlidingExpirationInMinutes = 60
            }
        };

        var config = new ExchangeRateProvidersConfig
        {
            CzechNationalBank = czechConfig
        };

        Assert.NotNull(config);
        Assert.NotNull(config.CzechNationalBank);
        Assert.Equal(30, config.CzechNationalBank.Cache.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(10, config.CzechNationalBank.Cache.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(1440, config.CzechNationalBank.Cache.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(60, config.CzechNationalBank.Cache.MonthlyRatesSlidingExpirationInMinutes);
    }

    [Fact]
    public void ExchangeRateProvidersConfig_WithNullCzechNationalBank_ShouldCreateSuccessfully()
    {
        var config = new ExchangeRateProvidersConfig
        {
            CzechNationalBank = null
        };

        Assert.NotNull(config);
        Assert.Null(config.CzechNationalBank);
    }

    [Fact]
    public void ExchangeRateProvidersConfig_WithDefaultValues_ShouldCreateSuccessfully()
    {
        var config = new ExchangeRateProvidersConfig();

        Assert.NotNull(config);
        Assert.Null(config.CzechNationalBank);
    }
} 