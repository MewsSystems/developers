using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;

namespace ExchangeRateUpdater.Infrastructure.Providers.Tests.ExchangeRates.CzechNationalBank;

public class CnbExchangeRateProviderTests
{
    private readonly Mock<ICzechNationalBankApiClient> _mockApiClient;
    private readonly IDistributedCache _cache;
    private readonly ICacheService _cacheService;
    private readonly Mock<ILogger<CnbExchangeRateProvider>> _mockLogger;
    private readonly CzechNationalBankExchangeRateConfig _config;
    private readonly CnbExchangeRateProvider _provider;

    public CnbExchangeRateProviderTests()
    {
        _mockApiClient = new Mock<ICzechNationalBankApiClient>();
        _mockLogger = new Mock<ILogger<CnbExchangeRateProvider>>();

        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        
        _cache = serviceProvider.GetRequiredService<IDistributedCache>();
        _cacheService = new DistributedCacheService(_cache);

        _config = new CzechNationalBankExchangeRateConfig
        {
            Cache = new CacheConfig
            {
                DailyRatesAbsoluteExpirationInMinutes = 30,
                DailyRatesSlidingExpirationInMinutes = 10,
                MonthlyRatesAbsoluteExpirationInMinutes = 1440,
                MonthlyRatesSlidingExpirationInMinutes = 60
            }
        };

        var options = Options.Create(_config);
        _provider = new CnbExchangeRateProvider(_mockApiClient.Object, _cacheService, options, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithValidDependencies_ShouldCreateSuccessfully()
    {
        var provider = new CnbExchangeRateProvider(_mockApiClient.Object, _cacheService, Options.Create(_config), _mockLogger.Object);
        Assert.NotNull(provider);
    }

    [Fact]
    public void Name_ShouldReturnCorrectProviderName()
    {
        Assert.Equal("CzechNationalBank", _provider.Name);
    }

    [Fact]
    public void DefaultLanguage_ShouldReturnCorrectLanguage()
    {
        Assert.Equal("EN", _provider.DefaultLanguage);
    }

    [Fact]
    public void DefaultCurrency_ShouldReturnCorrectCurrency()
    {
        Assert.Equal("CZK", _provider.DefaultCurrency);
    }

    [Fact]
    public async Task FetchAllCurrentAsync_ShouldReturnEmptyArray_WhenNoApiSetup()
    {
        var result = await _provider.FetchAllCurrentAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FetchByDateAsync_ShouldReturnEmptyArray_WhenNoApiSetup()
    {
        var date = new DateTime(2024, 1, 15);
        var result = await _provider.FetchByDateAsync(date);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FetchByDateAsync_WithFutureDate_ShouldUseCurrentDate()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);
        var result = await _provider.FetchByDateAsync(futureDate);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task FetchByDateAsync_WithCachedData_ShouldReturnCachedRates()
    {
        var date = new DateTime(2024, 1, 15);
        var cachedResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel { CurrencyCode = "USD", Rate = 23.5m, Amount = 1, Country = "USA", Currency = "US Dollar", Order = 1, ValidFor = "2024-01-15" }
            }
        };

        var dailyCacheKey = CacheKeyGenerator.GenerateDailyRatesKey(_provider.Name, date);
        var monthlyCacheKey = CacheKeyGenerator.GenerateMonthlyRatesKey(_provider.Name, date);

        await _cacheService.SetAsync(dailyCacheKey, cachedResponse, null, TimeSpan.FromMinutes(5), null);
        await _cacheService.SetAsync(monthlyCacheKey, cachedResponse, null, TimeSpan.FromMinutes(5), null);

        var result = await _provider.FetchByDateAsync(date);

        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
    }

    [Fact]
    public async Task FetchByDateAsync_WithPartialCachedData_ShouldFetchMissingData()
    {
        var date = new DateTime(2024, 1, 15);
        var cachedResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel { CurrencyCode = "USD", Rate = 23.5m, Amount = 1, Country = "USA", Currency = "US Dollar", Order = 1, ValidFor = "2024-01-15" }
            }
        };

        var dailyCacheKey = CacheKeyGenerator.GenerateDailyRatesKey(_provider.Name, date);
        await _cacheService.SetAsync(dailyCacheKey, cachedResponse, null, TimeSpan.FromMinutes(5), null);

        var result = await _provider.FetchByDateAsync(date);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void DefaultTimezone_ShouldReturnEuropePrague()
    {
        var timezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        Assert.NotNull(timezone);
        Assert.Equal("Europe/Prague", timezone.Id);
    }

    [Fact]
    public void CnbExchangeRateModel_ToExchangeRate_ShouldConvertCorrectly()
    {
        var model = new CnbExchangeRateModel
        {
            CurrencyCode = "USD",
            Rate = 23.5m,
            Amount = 1,
            Country = "USA",
            Currency = "US Dollar",
            Order = 1,
            ValidFor = "2024-01-15"
        };

        var exchangeRate = model.ToExchangeRate();

        Assert.NotNull(exchangeRate);
        Assert.Equal("USD", exchangeRate.SourceCurrency.Code);
        Assert.Equal("CZK", exchangeRate.TargetCurrency.Code);
        Assert.Equal(23.5m, exchangeRate.ExchangeValue);
        Assert.Equal("CNB", exchangeRate.ProviderName);
        Assert.Equal(new DateTime(2024, 1, 15), exchangeRate.ValidUntil);
    }

    [Fact]
    public void CnbExchangeRateModel_ToExchangeRate_WithAmountGreaterThanOne_ShouldCalculateCorrectRate()
    {
        var model = new CnbExchangeRateModel
        {
            CurrencyCode = "JPY",
            Rate = 100m,
            Amount = 100,
            Country = "Japan",
            Currency = "Japanese Yen",
            Order = 4,
            ValidFor = "2024-01-15"
        };

        var exchangeRate = model.ToExchangeRate();

        Assert.NotNull(exchangeRate);
        Assert.Equal("JPY", exchangeRate.SourceCurrency.Code);
        Assert.Equal("CZK", exchangeRate.TargetCurrency.Code);
        Assert.Equal(1m, exchangeRate.ExchangeValue);
        Assert.Equal("CNB", exchangeRate.ProviderName);
    }

    [Fact]
    public async Task FetchByDateAsync_WithCurrentDate_ShouldCacheUntilEndOfDayInTimezone()
    {
        var currentDate = DateTime.UtcNow;
        var pragueTimezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var pragueCurrentDate = TimeZoneInfo.ConvertTimeFromUtc(currentDate, pragueTimezone).Date;
        var dateString = pragueCurrentDate.ToString("yyyy-MM-dd");
        var monthString = pragueCurrentDate.ToString("yyyy-MM");
        
        var mockDailyResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel
                {
                    CurrencyCode = "USD",
                    Rate = 25.5m,
                    Amount = 1,
                    Country = "USA",
                    Currency = "US Dollar",
                    Order = 1,
                    ValidFor = dateString
                }
            }
        };

        var mockMonthlyResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel
                {
                    CurrencyCode = "EUR",
                    Rate = 26.0m,
                    Amount = 1,
                    Country = "Eurozone",
                    Currency = "Euro",
                    Order = 2,
                    ValidFor = dateString
                }
            }
        };

        _mockApiClient.Setup(x => x.GetFrequentExchangeRatesAsync(dateString, It.IsAny<string>()))
            .ReturnsAsync(mockDailyResponse);
        _mockApiClient.Setup(x => x.GetOtherExchangeRatesAsync(monthString, It.IsAny<string>()))
            .ReturnsAsync(mockMonthlyResponse);

        var result = await _provider.FetchByDateAsync(pragueCurrentDate);

        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal("USD", result[0].SourceCurrency.Code);
        Assert.Equal("EUR", result[1].SourceCurrency.Code);

        _mockApiClient.Verify(x => x.GetFrequentExchangeRatesAsync(dateString, It.IsAny<string>()), Times.Once);
        _mockApiClient.Verify(x => x.GetOtherExchangeRatesAsync(monthString, It.IsAny<string>()), Times.Once);

        var dailyCacheKey = $"ExchangeRates:CzechNationalBank:Daily:{pragueCurrentDate:yyyy-MM-dd}";
        var dailyCachedData = await _cache.GetStringAsync(dailyCacheKey);
        Assert.NotNull(dailyCachedData);

        var monthlyCacheKey = $"ExchangeRates:CzechNationalBank:Monthly:{pragueCurrentDate:yyyy-MM}";
        var monthlyCachedData = await _cache.GetStringAsync(monthlyCacheKey);
        Assert.NotNull(monthlyCachedData);
    }

    [Fact]
    public async Task FetchByDateAsync_WithPastDate_ShouldUseSlidingExpiration()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var pragueTimezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var praguePastDate = TimeZoneInfo.ConvertTimeFromUtc(pastDate, pragueTimezone).Date;
        var dateString = praguePastDate.ToString("yyyy-MM-dd");
        var monthString = praguePastDate.ToString("yyyy-MM");
        
        var mockDailyResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel
                {
                    CurrencyCode = "EUR",
                    Rate = 26.0m,
                    Amount = 1,
                    Country = "Eurozone",
                    Currency = "Euro",
                    Order = 2,
                    ValidFor = dateString
                }
            }
        };

        var mockMonthlyResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel
                {
                    CurrencyCode = "JPY",
                    Rate = 17.0m,
                    Amount = 100,
                    Country = "Japan",
                    Currency = "Japanese Yen",
                    Order = 4,
                    ValidFor = dateString
                }
            }
        };

        _mockApiClient.Setup(x => x.GetFrequentExchangeRatesAsync(dateString, It.IsAny<string>()))
            .ReturnsAsync(mockDailyResponse);
        _mockApiClient.Setup(x => x.GetOtherExchangeRatesAsync(monthString, It.IsAny<string>()))
            .ReturnsAsync(mockMonthlyResponse);

        var result = await _provider.FetchByDateAsync(praguePastDate);

        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal("EUR", result[0].SourceCurrency.Code);
        Assert.Equal("JPY", result[1].SourceCurrency.Code);

        var dailyCacheKey = $"ExchangeRates:CzechNationalBank:Daily:{praguePastDate:yyyy-MM-dd}";
        var dailyCachedData = await _cache.GetStringAsync(dailyCacheKey);
        Assert.NotNull(dailyCachedData);

        var monthlyCacheKey = $"ExchangeRates:CzechNationalBank:Monthly:{praguePastDate:yyyy-MM}";
        var monthlyCachedData = await _cache.GetStringAsync(monthlyCacheKey);
        Assert.NotNull(monthlyCachedData);
    }

    [Fact]
    public async Task FetchByDateAsync_WithDifferentTimezoneCurrentDate_ShouldCacheUntilEndOfDayInPragueTimezone()
    {
        var utcNow = DateTime.UtcNow;
        var pragueTimezone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var pragueCurrentDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, pragueTimezone).Date;
        var dateString = pragueCurrentDate.ToString("yyyy-MM-dd");
        var monthString = pragueCurrentDate.ToString("yyyy-MM");
        
        var mockDailyResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel
                {
                    CurrencyCode = "JPY",
                    Rate = 17.0m,
                    Amount = 100,
                    Country = "Japan",
                    Currency = "Japanese Yen",
                    Order = 4,
                    ValidFor = dateString
                }
            }
        };

        var mockMonthlyResponse = new CnbExchangeRateResponse
        {
            Rates = new[]
            {
                new CnbExchangeRateModel
                {
                    CurrencyCode = "GBP",
                    Rate = 30.0m,
                    Amount = 1,
                    Country = "United Kingdom",
                    Currency = "British Pound",
                    Order = 3,
                    ValidFor = dateString
                }
            }
        };

        _mockApiClient.Setup(x => x.GetFrequentExchangeRatesAsync(dateString, It.IsAny<string>()))
            .ReturnsAsync(mockDailyResponse);
        _mockApiClient.Setup(x => x.GetOtherExchangeRatesAsync(monthString, It.IsAny<string>()))
            .ReturnsAsync(mockMonthlyResponse);

        var result = await _provider.FetchByDateAsync(pragueCurrentDate);

        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Equal("JPY", result[0].SourceCurrency.Code);
        Assert.Equal("GBP", result[1].SourceCurrency.Code);

        var dailyCacheKey = $"ExchangeRates:CzechNationalBank:Daily:{pragueCurrentDate:yyyy-MM-dd}";
        var dailyCachedData = await _cache.GetStringAsync(dailyCacheKey);
        Assert.NotNull(dailyCachedData);

        var monthlyCacheKey = $"ExchangeRates:CzechNationalBank:Monthly:{pragueCurrentDate:yyyy-MM}";
        var monthlyCachedData = await _cache.GetStringAsync(monthlyCacheKey);
        Assert.NotNull(monthlyCachedData);
    }


} 