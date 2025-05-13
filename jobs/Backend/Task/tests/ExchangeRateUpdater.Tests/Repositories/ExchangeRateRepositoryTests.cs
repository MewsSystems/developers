using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace ExchangeRateUpdater.Tests.Repositories;

public class ExchangeRateRepositoryTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ExchangeRateRepository> _logger;
    public ExchangeRateRepositoryTests()
    {
        _memoryCache = Substitute.For<IMemoryCache>();
        _logger = Substitute.For<ILogger<ExchangeRateRepository>>();
    }

    [Fact]
    public async Task GetCzkExchangeRatesAsync_ShouldReturnRatesFromCache_WhenCacheHit()
    {
        // Arrange
        ICnbExchangeRateClient cnbExchangeRateClient = Substitute.For<ICnbExchangeRateClient>();

        ExchangeRate commonCachedExchangeRate = new(new Currency("USD"), new Currency("CZK"), 23.5m);
        ExchangeRate uncommonCachedExchangeRate = new(new Currency("EUR"), new Currency("CZK"), 23.5m);
        IEnumerable<ExchangeRate>? commonCacheValue =
        [
            commonCachedExchangeRate
        ];
        IEnumerable<ExchangeRate>? uncommonCacheValue =
        [
            uncommonCachedExchangeRate
        ];

        string commonCacheKey = ExchangeRateCacheHelper.Cnb.CommonRates.CacheKey;
        _memoryCache
        .TryGetValue(commonCacheKey, out Arg.Any<object?>())
        .Returns(call =>
        {
            call[1] = commonCacheValue;
            return true;
        });

        string uncommonCacheKey = ExchangeRateCacheHelper.Cnb.UncommonRates.CacheKey;
        _memoryCache
        .TryGetValue(uncommonCacheKey, out Arg.Any<object?>())
        .Returns(call =>
        {
            call[1] = uncommonCacheValue;
            return true;
        });

        ExchangeRateRepository sut = new(_memoryCache, cnbExchangeRateClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = await sut.GetCzkExchangeRatesAsync();

        // Assert
        exchangeRates.Count().ShouldBe(2);
        exchangeRates.Any(x => x.SourceCurrency.Code == commonCachedExchangeRate.SourceCurrency.Code).ShouldBeTrue();
        exchangeRates.Any(x => x.SourceCurrency.Code == uncommonCachedExchangeRate.SourceCurrency.Code).ShouldBeTrue();
        await cnbExchangeRateClient.DidNotReceive().FetchCommonExchangeRatesAsync(Arg.Any<CancellationToken>());
        await cnbExchangeRateClient.DidNotReceive().FetchUncommonExchangeRatesAsync(Arg.Any<CancellationToken>());

        _logger.Received(2).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Cache hit for")),
            null,
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Fact]
    public async Task GetCzkExchangeRatesAsync_ShouldFetchRatesFromService_WhenCacheMiss()
    {
        // Arrange
        ICnbExchangeRateClient cnbExchangeRateClient = Substitute.For<ICnbExchangeRateClient>();

        ExchangeRate commonExchangeRate = new(new Currency("USD"), new Currency("CZK"), 23.5m);
        ExchangeRate uncommonExchangeRate = new(new Currency("EUR"), new Currency("CZK"), 23.5m);
        IEnumerable<ExchangeRate> commonExchangeRates =
        [
            commonExchangeRate
        ];
        IEnumerable<ExchangeRate> uncommonExchangeRates =
        [
            uncommonExchangeRate
        ];

        string commonCacheKey = ExchangeRateCacheHelper.Cnb.CommonRates.CacheKey;
        _memoryCache
        .TryGetValue(commonCacheKey, out Arg.Any<object?>())
        .Returns(false);

        string uncommonCacheKey = ExchangeRateCacheHelper.Cnb.UncommonRates.CacheKey;
        _memoryCache
        .TryGetValue(uncommonCacheKey, out Arg.Any<object?>())
        .Returns(false);

        cnbExchangeRateClient.FetchCommonExchangeRatesAsync().Returns(commonExchangeRates);
        cnbExchangeRateClient.FetchUncommonExchangeRatesAsync().Returns(uncommonExchangeRates);

        ExchangeRateRepository sut = new(_memoryCache, cnbExchangeRateClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = await sut.GetCzkExchangeRatesAsync();

        // Assert
        exchangeRates.Count().ShouldBe(2);
        exchangeRates.Any(x => x.SourceCurrency.Code == commonExchangeRate.SourceCurrency.Code).ShouldBeTrue();
        exchangeRates.Any(x => x.SourceCurrency.Code == uncommonExchangeRate.SourceCurrency.Code).ShouldBeTrue();
        await cnbExchangeRateClient.Received(1).FetchCommonExchangeRatesAsync(Arg.Any<CancellationToken>());
        await cnbExchangeRateClient.Received(1).FetchUncommonExchangeRatesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCzkExchangeRatesAsync_ShouldReturnRatesFromCacheAndService_WhenCacheHitAndMiss()
    {
        // Arrange
        ICnbExchangeRateClient cnbExchangeRateClient = Substitute.For<ICnbExchangeRateClient>();

        ExchangeRate commonCachedExchangeRate = new(new Currency("USD"), new Currency("CZK"), 23.5m);
        ExchangeRate uncommonExchangeRate = new(new Currency("EUR"), new Currency("CZK"), 23.5m);
        IEnumerable<ExchangeRate>? commonCacheValue =
        [
            commonCachedExchangeRate
        ];
        IEnumerable<ExchangeRate> uncommonExchangeRates =
        [
            uncommonExchangeRate
        ];

        string commonCacheKey = ExchangeRateCacheHelper.Cnb.CommonRates.CacheKey;
        _memoryCache
        .TryGetValue(commonCacheKey, out Arg.Any<object?>())
        .Returns(call =>
        {
            call[1] = commonCacheValue;
            return true;
        });

        string uncommonCacheKey = ExchangeRateCacheHelper.Cnb.UncommonRates.CacheKey;
        _memoryCache
        .TryGetValue(uncommonCacheKey, out Arg.Any<object?>())
        .Returns(false);

        cnbExchangeRateClient.FetchUncommonExchangeRatesAsync().Returns(uncommonExchangeRates);

        ExchangeRateRepository sut = new(_memoryCache, cnbExchangeRateClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = await sut.GetCzkExchangeRatesAsync();

        // Assert
        exchangeRates.Count().ShouldBe(2);
        exchangeRates.Any(x => x.SourceCurrency.Code == commonCachedExchangeRate.SourceCurrency.Code).ShouldBeTrue();
        exchangeRates.Any(x => x.SourceCurrency.Code == uncommonExchangeRate.SourceCurrency.Code).ShouldBeTrue();
        await cnbExchangeRateClient.DidNotReceive().FetchCommonExchangeRatesAsync(Arg.Any<CancellationToken>());
        await cnbExchangeRateClient.Received().FetchUncommonExchangeRatesAsync(Arg.Any<CancellationToken>());

        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Cache hit for")),
            null,
            Arg.Any<Func<object, Exception?, string>>()
        );
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Cache miss for")),
            null,
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Fact]
    public async Task GetCzkExchangeRatesAsync_ShouldPopulateCache_WhenCacheMiss()
    {
        // Arrange
        ICnbExchangeRateClient cnbExchangeRateClient = Substitute.For<ICnbExchangeRateClient>();
        string commonCacheKey = ExchangeRateCacheHelper.Cnb.CommonRates.CacheKey;
        string uncommonCacheKey = ExchangeRateCacheHelper.Cnb.UncommonRates.CacheKey;

        _memoryCache
        .TryGetValue(Arg.Any<string>(), out Arg.Any<object?>())
        .Returns(false);

        ExchangeRateRepository sut = new(_memoryCache, cnbExchangeRateClient, _logger);

        // Act
        await sut.GetCzkExchangeRatesAsync();

        // Assert
        _memoryCache.Received(1).Set(commonCacheKey, Arg.Any<object>);
        _memoryCache.Received(1).Set(uncommonCacheKey, Arg.Any<object>);
    }

    [Fact]
    public async Task GetCzkExchangeRatesAsync_ShouldReturnEmpty_WhenExternalServiceFails()
    {
        // Arrange
        ICnbExchangeRateClient cnbExchangeRateClient = Substitute.For<ICnbExchangeRateClient>();

        string commonCacheKey = ExchangeRateCacheHelper.Cnb.CommonRates.CacheKey;
        _memoryCache
        .TryGetValue(commonCacheKey, out Arg.Any<object?>())
        .Returns(false);

        string uncommonCacheKey = ExchangeRateCacheHelper.Cnb.UncommonRates.CacheKey;
        _memoryCache
        .TryGetValue(uncommonCacheKey, out Arg.Any<object?>())
        .Returns(false);

        cnbExchangeRateClient.FetchCommonExchangeRatesAsync().ThrowsAsync(new Exception());
        cnbExchangeRateClient.FetchUncommonExchangeRatesAsync().ThrowsAsync(new Exception());

        ExchangeRateRepository sut = new(_memoryCache, cnbExchangeRateClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = await sut.GetCzkExchangeRatesAsync();

        // Assert
        exchangeRates.ShouldBeEmpty();
        await cnbExchangeRateClient.Received(1).FetchCommonExchangeRatesAsync(Arg.Any<CancellationToken>());
        await cnbExchangeRateClient.Received(1).FetchUncommonExchangeRatesAsync(Arg.Any<CancellationToken>());

        _logger.Received(2).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("Failed to retrieve latests exchange rates for")),
            Arg.Is<Exception>(ex => ex is Exception),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
