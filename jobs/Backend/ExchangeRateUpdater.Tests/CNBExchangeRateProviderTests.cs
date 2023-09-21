using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Net;

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
        configurationMock.Setup(config => config["CNBApi:DateFormat"]).Returns("yyyy-MM-dd");

        // Create an instance of CNBExchangeRateProvider with the mocked dependencies
        var provider = new CNBExchangeRateProvider(configurationMock.Object, null, memoryCache, loggerMock.Object);

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
    public async Task GetDailyExchangeRateAsync_NoDateSpecified_UseTodaysDate()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var loggerMock = new Mock<ILogger<CNBExchangeRateProvider>>();

        // Create an in-memory cache for testing
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        // Configure the configuration mock to return the expected values
        configurationMock.Setup(config => config["CNBApi:DateFormat"]).Returns("yyyy-MM-dd");

        var provider = new CNBExchangeRateProvider(configurationMock.Object, null, memoryCache, loggerMock.Object);

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

    [Fact]
    public async Task GetDailyExchangeRateAsync_NoCacheHit_ApiCallIsMade()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var loggerMock = new Mock<ILogger<CNBExchangeRateProvider>>();

        configurationMock.Setup(config => config["CNBApi:BaseUrl"]).Returns("https://example.com/");
        configurationMock.Setup(config => config["CNBApi:ExchangeRateEndpoint"]).Returns("exchangeRates/");
        configurationMock.Setup(config => config["CNBApi:DateFormat"]).Returns("yyyy-MM-dd");
        configurationMock.Setup(config => config["CNBApi:DefaultCurrency"]).Returns("CZK");

        var date = DateTime.Today;

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(() =>
        {
            var httpClientHandler = new TestHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ExchangeRateApiData
                {
                    Rates = new[]
                    {
                    new ExchangeRateItem { CurrencyCode = "USD", ValidFor = date.ToString("yyyy-MM-dd"), Rate = 25.0m },
                    new ExchangeRateItem { CurrencyCode = "EUR", ValidFor = date.ToString("yyyy-MM-dd"), Rate = 30.0m }
                }
                }))
            });

            return new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri("https://example.com/")
            };
        });

        var provider = new CNBExchangeRateProvider(configurationMock.Object, httpClientFactoryMock.Object, cache, loggerMock.Object);

        var currencies = new[] { new Currency("USD"), new Currency("EUR") };

        // Act
        var result = await provider.GetDailyExchangeRateAsync(currencies, date);

        // Assert
        httpClientFactoryMock.Verify(factory => factory.CreateClient(It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task GetDailyExchangeRateAsync_OneCacheHit_TwoCacheMisses_ApiCallIsMadeTwice()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var loggerMock = new Mock<ILogger<CNBExchangeRateProvider>>();

        configurationMock.Setup(config => config["CNBApi:BaseUrl"]).Returns("https://example.com/");
        configurationMock.Setup(config => config["CNBApi:ExchangeRateEndpoint"]).Returns("exchangeRates/");
        configurationMock.Setup(config => config["CNBApi:DateFormat"]).Returns("yyyy-MM-dd");
        configurationMock.Setup(config => config["CNBApi:DefaultCurrency"]).Returns("CZK");

        var date = DateTime.Today;
        var secondDate = DateTime.Today.AddDays(-2);

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(() =>
        {
            var httpClientHandler = new TestHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ExchangeRateApiData
                {
                    Rates = new[]
                    {
                    new ExchangeRateItem { CurrencyCode = "USD", ValidFor = date.ToString("yyyy-MM-dd"), Rate = 25.0m },
                    new ExchangeRateItem { CurrencyCode = "EUR", ValidFor = date.ToString("yyyy-MM-dd"), Rate = 30.0m }
                }
                }))
            });

            return new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri("https://example.com/")
            };
        });

        var provider = new CNBExchangeRateProvider(configurationMock.Object, httpClientFactoryMock.Object, cache, loggerMock.Object);

        var currencies = new[] { new Currency("USD"), new Currency("EUR") };

        // Act
        var result = await provider.GetDailyExchangeRateAsync(currencies, date);
        var secondResult = await provider.GetDailyExchangeRateAsync(currencies, secondDate);
        var thirdResult = await provider.GetDailyExchangeRateAsync(currencies, date);

        // Assert
        httpClientFactoryMock.Verify(factory => factory.CreateClient(It.IsAny<string>()), Times.Exactly(2));
    }
}