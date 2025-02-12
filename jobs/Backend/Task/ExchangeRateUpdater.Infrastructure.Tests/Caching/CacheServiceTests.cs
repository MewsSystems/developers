using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Infrastructure.Tests.Caching;

public class CacheServiceTests
{
    private readonly Mock<ILogger<CacheService>> _loggerMock;
    private readonly Mock<IOptions<CacheSettings>> _cacheSettingsMock;
    private readonly MemoryCache _memoryCache;
    private readonly CacheService _cacheService;

    public CacheServiceTests()
    {
        _loggerMock = new Mock<ILogger<CacheService>>();
        _cacheSettingsMock = new Mock<IOptions<CacheSettings>>();
        _cacheSettingsMock.Setup(cs => cs.Value).Returns(new CacheSettings { Duration = 30 });

        // Use a real MemoryCache for proper testing behavior
        _memoryCache = new MemoryCache(new MemoryCacheOptions());

        _cacheService = new CacheService(_memoryCache, _loggerMock.Object, _cacheSettingsMock.Object);
    }

    [Fact]
    public async Task GetOrCreateAsync_ReturnsCachedData_WhenAvailable()
    {
        // Arrange
        var cacheKey = "testKey";
        var expectedData = "CachedValue";
        _memoryCache.Set(cacheKey, expectedData);

        // Act
        var result = await _cacheService.GetOrCreateAsync(cacheKey, () => Task.FromResult("NewValue"));

        // Assert
        Assert.Equal(expectedData, result);
    }

    [Fact]
    public async Task GetOrCreateAsync_CallsFetchFunction_WhenCacheIsEmpty()
    {
        // Arrange
        var cacheKey = "testKey";
        var expectedData = "FetchedValue";

        // Act
        var result = await _cacheService.GetOrCreateAsync(cacheKey, () => Task.FromResult(expectedData));

        // Assert
        Assert.Equal(expectedData, result);

        // Verify that the value was actually stored in the cache
        Assert.True(_memoryCache.TryGetValue(cacheKey, out var cachedValue));
        Assert.Equal(expectedData, cachedValue);
    }

    [Fact]
    public async Task GetOrCreateAsync_DoesNotCacheNullData()
    {
        // Arrange
        var cacheKey = "testKey";

        // Act
        var result = await _cacheService.GetOrCreateAsync<string>(cacheKey, () => Task.FromResult<string?>(null));

        // Assert
        Assert.Null(result);
        Assert.False(_memoryCache.TryGetValue(cacheKey, out _));
    }

    [Fact]
    public async Task GetOrCreateAsync_EnsuresOnlyOneFetchFunctionExecutesForSameKey()
    {
        // Arrange
        var cacheKey = "testKey";
        var fetchCounter = 0;

        async Task<string> FetchFunction()
        {
            fetchCounter++;
            await Task.Delay(100); // Simulate delay in fetching
            return "FetchedValue";
        }

        // Act: Call GetOrCreateAsync multiple times in parallel
        var task1 = _cacheService.GetOrCreateAsync(cacheKey, FetchFunction);
        var task2 = _cacheService.GetOrCreateAsync(cacheKey, FetchFunction);
        var results = await Task.WhenAll(task1, task2);

        // Assert
        Assert.Equal("FetchedValue", results[0]);
        Assert.Equal("FetchedValue", results[1]);
        Assert.Equal(1, fetchCounter); // Ensure fetchFunction was called only once
    }

    [Fact]
    public async Task GetOrCreateAsync_AllowsMultipleFetchesForDifferentKeys()
    {
        // Arrange
        var key1 = "key1";
        var key2 = "key2";
        var value1 = "Value1";
        var value2 = "Value2";

        // Act
        var task1 = _cacheService.GetOrCreateAsync(key1, () => Task.FromResult(value1));
        var task2 = _cacheService.GetOrCreateAsync(key2, () => Task.FromResult(value2));

        var result1 = await task1;
        var result2 = await task2;

        // Assert
        Assert.Equal(value1, result1);
        Assert.Equal(value2, result2);
    }
}
