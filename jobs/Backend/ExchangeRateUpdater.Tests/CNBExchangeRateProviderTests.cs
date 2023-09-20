using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Providers.Tests;

public class CNBExchangeRateProviderTests
{
    [Fact]
    public async Task GetDailyExchangeRateAsync_CacheHit_ReturnsRequiredCachedRates()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<CNBExchangeRateProvider>>();

        // Create an in-memory cache for testing
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        // Configure the configuration mock to return the expected values
        configurationMock.Setup(config => config["CNBApi:BaseUrl"]).Returns("https://example.com/");
        configurationMock.Setup(config => config["CNBApi:ExchangeRateEndpoint"]).Returns("exchangeRates");
        configurationMock.Setup(config => config["CNBApi:DateFormat"]).Returns("yyyy-MM-dd");
        configurationMock.Setup(config => config["CNBApi:DefaultCurrency"]).Returns("CZK");

        // Create an instance of CNBExchangeRateProvider with the mocked dependencies
        var provider = new CNBExchangeRateProvider(null, configurationMock.Object, memoryCache, loggerMock.Object);

        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var date = DateTime.Today.Date;
        string cacheKey = $"ExchangeRates_{date:yyyy-MM-dd}";

        // Create cached rates and add them to the memory cache
        var cachedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 25.0m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 30.0m),
                new ExchangeRate(new Currency("CZK"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 1.0m),
                new ExchangeRate(new Currency("JPY"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 0.15m),
            };

        memoryCache.Set(cacheKey, cachedRates);

        // Act
        var result = await provider.GetDailyExchangeRateAsync(currencies, date);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("USD", result.First().SourceCurrency.Code);
        Assert.Equal("EUR", result.Last().SourceCurrency.Code);
    }

    [Fact]
    public async Task GetDailyExchangeRateAsync_NoDateSpecified_UseTodayDate()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<CNBExchangeRateProvider>>();

        // Create an in-memory cache for testing
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        // Configure the configuration mock to return the expected values
        configurationMock.Setup(config => config["CNBApi:BaseUrl"]).Returns("https://example.com/");
        configurationMock.Setup(config => config["CNBApi:ExchangeRateEndpoint"]).Returns("exchangeRates");
        configurationMock.Setup(config => config["CNBApi:DateFormat"]).Returns("yyyy-MM-dd");
        configurationMock.Setup(config => config["CNBApi:DefaultCurrency"]).Returns("CZK");

        var provider = new CNBExchangeRateProvider(null, configurationMock.Object, memoryCache, loggerMock.Object);

        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var date = DateTime.Today.Date;
        string cacheKey = $"ExchangeRates_{date:yyyy-MM-dd}";

        // Create cached rates and add them to the memory cache
        var cachedRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 25.0m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 30.0m),
                new ExchangeRate(new Currency("CZK"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 1.0m),
                new ExchangeRate(new Currency("JPY"), new Currency("CZK"), date.ToString("yyyy-MM-dd"), 0.15m),
            };

        memoryCache.Set(cacheKey, cachedRates);

        // Act
        var result = await provider.GetDailyExchangeRateAsync(currencies);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        // Ensure that the date used is Today
        Assert.All(result, rate => Assert.Equal(DateTime.Today.ToString("yyyy-MM-dd"), rate.ValidFor));
    }
}