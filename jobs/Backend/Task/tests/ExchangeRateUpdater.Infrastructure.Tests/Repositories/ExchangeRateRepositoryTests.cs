using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Moq;

namespace ExchangeRateUpdater.Infrastructure.Tests.Repositories;

public class ExchangeRateRepositoryTests
{
    private readonly Mock<IExchangeRateProvider> _mockProvider1;
    private readonly Mock<IExchangeRateProvider> _mockProvider2;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<ILogger<ExchangeRateRepository>> _mockLogger;
    private readonly Mock<IOptions<ExchangeRateProvidersConfig>> _mockConfig;
    private readonly ExchangeRateRepository _repository;

    public ExchangeRateRepositoryTests()
    {
        _mockProvider1 = new Mock<IExchangeRateProvider>();
        _mockProvider2 = new Mock<IExchangeRateProvider>();
        _mockCacheService = new Mock<ICacheService>();
        _mockLogger = new Mock<ILogger<ExchangeRateRepository>>();
        _mockConfig = new Mock<IOptions<ExchangeRateProvidersConfig>>();

        _mockProvider1.Setup(p => p.Name).Returns("Provider1");
        _mockProvider2.Setup(p => p.Name).Returns("Provider2");
        _mockProvider1.Setup(p => p.TimeZone).Returns(TimeZoneInfo.Utc);
        _mockProvider2.Setup(p => p.TimeZone).Returns(TimeZoneInfo.Utc);

        // Setup default configuration
        var defaultConfig = new ExchangeRateProvidersConfig
        {
            CzechNationalBank = new CzechNationalBankExchangeRateConfig
            {
                Cache = new CacheConfig
                {
                    DailyRatesAbsoluteExpirationInMinutes = 30,
                    DailyRatesSlidingExpirationInMinutes = 10,
                    MonthlyRatesAbsoluteExpirationInMinutes = 1440,
                    MonthlyRatesSlidingExpirationInMinutes = 60
                }
            }
        };
        _mockConfig.Setup(c => c.Value).Returns(defaultConfig);

        var providers = new[] { _mockProvider1.Object, _mockProvider2.Object };
        _repository = new ExchangeRateRepository(providers, _mockCacheService.Object, _mockConfig.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithValidProviders_ShouldCreateSuccessfully()
    {
        var providers = new[] { _mockProvider1.Object };

        var repository = new ExchangeRateRepository(providers, _mockCacheService.Object, _mockConfig.Object, _mockLogger.Object);

        Assert.NotNull(repository);
    }

    [Fact]
    public void Constructor_WithEmptyProviders_ShouldCreateSuccessfully()
    {
        var providers = Array.Empty<IExchangeRateProvider>();

        var repository = new ExchangeRateRepository(providers, _mockCacheService.Object, _mockConfig.Object, _mockLogger.Object);

        Assert.NotNull(repository);
    }

    [Fact]
    public async Task FilterAsync_WithValidCurrencies_ShouldReturnFilteredRates()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        var result = await _repository.FilterAsync(currencies);

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task FilterAsync_WithEmptyCurrencies_ShouldReturnAllRates()
    {
        var currencies = Array.Empty<Currency>();
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        var result = await _repository.FilterAsync(currencies);

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task FilterAsync_WhenProviderFails_ShouldContinueWithSuccessfulProviders()
    {
        var currencies = new[] { new Currency("USD") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider2", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ThrowsAsync(new Exception("Provider1 failed"));

        _mockProvider2.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        var result = await _repository.FilterAsync(currencies);

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Contains("Provider2", result.Keys);
        Assert.Empty(result["Provider1"]);
        Assert.Single(result["Provider2"]);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRates()
    {
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task GetFromProviderAsync_WithValidProvider_ShouldReturnProviderRates()
    {
        var currencies = new[] { new Currency("USD") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        var result = await _repository.GetFromProviderAsync("Provider1", currencies);

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task GetFromProviderAsync_WithNonExistentProvider_ShouldThrowArgumentException()
    {
        var currencies = new[] { new Currency("USD") };

        await Assert.ThrowsAsync<ArgumentException>(() => 
            _repository.GetFromProviderAsync("NonExistentProvider", currencies));
    }

    [Fact]
    public async Task GetFromProviderAsync_WithEmptyCurrencies_ShouldReturnAllProviderRates()
    {
        var currencies = Array.Empty<Currency>();
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1)),
            new ExchangeRate(new Currency("EUR"), new Currency("USD"), 1.18m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        var result = await _repository.GetFromProviderAsync("Provider1", currencies);

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Empty(result["Provider1"]);
    }

    [Fact]
    public async Task GetFromProviderAsync_WhenProviderFails_ShouldReturnEmptyResult()
    {
        var currencies = new[] { new Currency("USD") };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ThrowsAsync(new Exception("Provider failed"));

        var result = await _repository.GetFromProviderAsync("Provider1", currencies);

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Empty(result["Provider1"]);
    }

    [Fact]
    public void ConfigurationAccess_WithValidProvider_ShouldReturnCorrectCacheConfig()
    {
        var config = new ExchangeRateProvidersConfig
        {
            CzechNationalBank = new CzechNationalBankExchangeRateConfig
            {
                Cache = new CacheConfig
                {
                    DailyRatesAbsoluteExpirationInMinutes = 45,
                    DailyRatesSlidingExpirationInMinutes = 15,
                    MonthlyRatesAbsoluteExpirationInMinutes = 2880,
                    MonthlyRatesSlidingExpirationInMinutes = 120
                }
            }
        };

        var cacheConfig = config["CzechNationalBank:Cache"];

        Assert.NotNull(cacheConfig);
        Assert.Equal(45, cacheConfig.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(15, cacheConfig.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(2880, cacheConfig.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(120, cacheConfig.MonthlyRatesSlidingExpirationInMinutes);
    }

    [Fact]
    public void ConfigurationAccess_WithInvalidKey_ShouldReturnNull()
    {
        var config = new ExchangeRateProvidersConfig();

        var cacheConfig = config["InvalidProvider:Cache"];

        Assert.Null(cacheConfig);
    }

    [Fact]
    public void ConfigurationAccess_WithInvalidFormat_ShouldReturnNull()
    {
        var config = new ExchangeRateProvidersConfig();

        var cacheConfig = config["InvalidFormat"];

        Assert.Null(cacheConfig);
    }

    [Fact]
    public void ConfigurationAccess_WithWrongSection_ShouldReturnNull()
    {
        var config = new ExchangeRateProvidersConfig();

        var cacheConfig = config["CzechNationalBank:Api"];

        Assert.Null(cacheConfig);
    }

    [Fact]
    public async Task Caching_WithPastDateRates_ShouldUseConfigurableExpiration()
    {
        var pragueTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var praguePastDate = TimeZoneInfo.ConvertTimeFromUtc(pastDate, pragueTimeZone).Date;

        _mockProvider1.Setup(p => p.Name).Returns("CzechNationalBank");
        _mockProvider1.Setup(p => p.TimeZone).Returns(pragueTimeZone);

        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m, "CzechNationalBank", praguePastDate)
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        await _repository.GetAllAsync();

        var expectedAbsoluteExpiration = TimeSpan.FromMinutes(30);
        var expectedSlidingExpiration = TimeSpan.FromMinutes(10);

        _mockCacheService.Verify(c => c.SetAsync(
            It.IsAny<string>(),
            It.IsAny<ExchangeRate[]>(),
            null,
            expectedAbsoluteExpiration,
            expectedSlidingExpiration), Times.Once);
    }

    [Fact]
    public async Task Caching_WithFutureDateRates_ShouldUseConfigurableExpiration()
    {
        var pragueTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague");
        var futureDate = DateTime.UtcNow.AddDays(1);
        var pragueFutureDate = TimeZoneInfo.ConvertTimeFromUtc(futureDate, pragueTimeZone).Date;

        _mockProvider1.Setup(p => p.Name).Returns("CzechNationalBank");
        _mockProvider1.Setup(p => p.TimeZone).Returns(pragueTimeZone);

        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m, "CzechNationalBank", pragueFutureDate)
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        await _repository.GetAllAsync();

        var expectedAbsoluteExpiration = TimeSpan.FromMinutes(30);
        var expectedSlidingExpiration = TimeSpan.FromMinutes(10);

        _mockCacheService.Verify(c => c.SetAsync(
            It.IsAny<string>(),
            It.IsAny<ExchangeRate[]>(),
            null,
            expectedAbsoluteExpiration,
            expectedSlidingExpiration), Times.Once);
    }

    [Fact]
    public async Task Caching_WithUnknownProvider_ShouldUseConfigurableExpiration()
    {
        var utcTimeZone = TimeZoneInfo.Utc;
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var utcPastDate = TimeZoneInfo.ConvertTimeFromUtc(pastDate, utcTimeZone).Date;

        _mockProvider1.Setup(p => p.Name).Returns("UnknownProvider");
        _mockProvider1.Setup(p => p.TimeZone).Returns(utcTimeZone);

        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "UnknownProvider", utcPastDate)
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        await _repository.GetAllAsync();

        var expectedAbsoluteExpiration = TimeSpan.FromMinutes(30);
        var expectedSlidingExpiration = TimeSpan.FromMinutes(10);

        _mockCacheService.Verify(c => c.SetAsync(
            It.IsAny<string>(),
            It.IsAny<ExchangeRate[]>(),
            null,
            expectedAbsoluteExpiration,
            expectedSlidingExpiration), Times.Once);
    }

    [Fact]
    public async Task Caching_WithCachedData_ShouldReturnCachedData()
    {
        var cachedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync(cachedRates);

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
        Assert.Equal(cachedRates[0], result["Provider1"][0]);

        _mockProvider1.Verify(p => p.FetchAllCurrentAsync(), Times.Never);
    }

    [Fact]
    public async Task Caching_WithEmptyRates_ShouldNotCache()
    {
        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(Array.Empty<ExchangeRate>());

        await _repository.GetAllAsync();

        _mockCacheService.Verify(c => c.SetAsync(
            It.IsAny<string>(),
            It.IsAny<ExchangeRate[]>(),
            It.IsAny<DateTimeOffset?>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<TimeSpan?>()), Times.Never);
    }

    [Fact]
    public async Task Caching_WithDifferentTimezones_ShouldUseCorrectEndOfDay()
    {
        var tokyoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        var currentDate = DateTime.UtcNow;
        var tokyoCurrentDate = TimeZoneInfo.ConvertTimeFromUtc(currentDate, tokyoTimeZone).Date;
        var tokyoEndOfDay = tokyoCurrentDate.AddDays(1);

        _mockProvider1.Setup(p => p.Name).Returns("TokyoProvider");
        _mockProvider1.Setup(p => p.TimeZone).Returns(tokyoTimeZone);

        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("JPY"), 150.0m, "TokyoProvider", tokyoCurrentDate)
        };

        _mockCacheService.Setup(c => c.GetAsync<ExchangeRate[]>(It.IsAny<string>()))
            .ReturnsAsync((ExchangeRate[]?)null);
        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        await _repository.GetAllAsync();

        _mockCacheService.Verify(c => c.SetAsync(
            It.IsAny<string>(),
            It.IsAny<ExchangeRate[]>(),
            tokyoEndOfDay,
            null,
            null), Times.Once);
    }

    [Fact]
    public void ConfigurationAccess_WithMultipleProviders_ShouldReturnCorrectConfigs()
    {
        var config = new ExchangeRateProvidersConfig
        {
            CzechNationalBank = new CzechNationalBankExchangeRateConfig
            {
                Cache = new CacheConfig
                {
                    DailyRatesAbsoluteExpirationInMinutes = 45,
                    DailyRatesSlidingExpirationInMinutes = 15,
                    MonthlyRatesAbsoluteExpirationInMinutes = 2880,
                    MonthlyRatesSlidingExpirationInMinutes = 120
                }
            }
        };

        var czechConfig = config["CzechNationalBank:Cache"];
        var unknownConfig = config["UnknownProvider:Cache"];

        Assert.NotNull(czechConfig);
        Assert.Equal(45, czechConfig.DailyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(15, czechConfig.DailyRatesSlidingExpirationInMinutes);
        Assert.Equal(2880, czechConfig.MonthlyRatesAbsoluteExpirationInMinutes);
        Assert.Equal(120, czechConfig.MonthlyRatesSlidingExpirationInMinutes);

        Assert.Null(unknownConfig);
    }

    [Fact]
    public void ConfigurationAccess_WithMalformedKeys_ShouldReturnNull()
    {
        var config = new ExchangeRateProvidersConfig();

        Assert.Null(config["InvalidKey"]);
        Assert.Null(config["CzechNationalBank"]);
        Assert.Null(config["CzechNationalBank:InvalidSection"]);
        Assert.Null(config["CzechNationalBank:Cache:Extra"]);
    }
} 