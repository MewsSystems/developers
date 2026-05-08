using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models.Countries.CZE;
using ExchangeRateUpdater.Services.Cache;
using ExchangeRateUpdater.Services.Countries.CZE;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Tests;

public class StartupTests
{
    [Fact]
    public void ConfigureServices_RegistersExpectedServices()
    {
        // Arrange
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>
    {
        {"CacheSettings:Provider", "file"},
        {"CacheSettings:FileCachePath", "testcache.json"},
        {"ExchangeProviders:CZE:BaseUrl", "http://example.com"},
        {"ExchangeProviders:CZE:TtlInSeconds", "60"},
        {"ExchangeProviders:CZE:UpdateHourInLocalTime", "06:00:00"}
    };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        Startup.ConfigureServices(services, configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var app = provider.GetService<App>();
        Assert.NotNull(app);

        var dateTimeProvider = provider.GetService<IDateTimeProvider>();
        Assert.NotNull(dateTimeProvider);

        var cacheService = provider.GetService<ICacheService>();
        Assert.NotNull(cacheService);
        Assert.IsType<FileCacheService>(cacheService);


        var czeSettings = provider.GetService<Microsoft.Extensions.Options.IOptionsSnapshot<CzeSettings>>();
        Assert.NotNull(czeSettings);
        Assert.Equal("http://example.com", czeSettings.Value.BaseUrl);

        var httpClientFactory = provider.GetService<IHttpClientFactory>();
        Assert.NotNull(httpClientFactory);
        var httpClient = httpClientFactory.CreateClient(nameof(CzeExchangeRateProvider));
        Assert.NotNull(httpClient);
    }

    [Fact]
    public void AddCache_RegistersRedisCache_WhenProviderIsRedis()
    {
        // Arrange
        var services = new ServiceCollection();

        var inMemorySettings = new Dictionary<string, string>
    {
        {"CacheSettings:Provider", "redis"},
        {"CacheSettings:RedisConfiguration", "localhost:6379"}
    };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        Startup.ConfigureServices(services, configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var cacheService = provider.GetService<ICacheService>();
        Assert.NotNull(cacheService);
        Assert.IsType<RedisCacheService>(cacheService);
    }
}
