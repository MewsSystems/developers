using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.Tests.Services;

public class DistributedCacheServiceTests
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheService _cacheService;

    public DistributedCacheServiceTests()
    {
        var services = new ServiceCollection();
        services.AddDistributedMemoryCache();
        var serviceProvider = services.BuildServiceProvider();
        
        _cache = serviceProvider.GetRequiredService<IDistributedCache>();
        _cacheService = new DistributedCacheService(_cache);
    }

    [Fact]
    public void Constructor_WithValidCache_ShouldCreateSuccessfully()
    {
        var cacheService = new DistributedCacheService(_cache);
        Assert.NotNull(cacheService);
    }

    [Fact]
    public async Task GetAsync_WithExistingKey_ShouldReturnDeserializedValue()
    {
        const string key = "test-key";
        var expectedValue = new TestData { Name = "Test", Value = 123 };
        
        await _cacheService.SetAsync(key, expectedValue, null, TimeSpan.FromMinutes(5), null);

        var result = await _cacheService.GetAsync<TestData>(key);

        Assert.NotNull(result);
        Assert.Equal(expectedValue.Name, result.Name);
        Assert.Equal(expectedValue.Value, result.Value);
    }

    [Fact]
    public async Task GetAsync_WithNonExistentKey_ShouldReturnNull()
    {
        var key = "non-existent-key";

        var result = await _cacheService.GetAsync<TestData>(key);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_WithInvalidJson_ShouldReturnNull()
    {
        var key = "invalid-json-key";
        var invalidJson = "{ invalid json }";

        await _cache.SetStringAsync(key, invalidJson);

        var result = await _cacheService.GetAsync<TestData>(key);

        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_WithValidData_ShouldSerializeAndStore()
    {
        var key = "test-key";
        var value = new TestData { Name = "Test", Value = 123 };
        var expiration = TimeSpan.FromMinutes(30);

        await _cacheService.SetAsync(key, value, null, expiration, null);

        var result = await _cacheService.GetAsync<TestData>(key);
        Assert.NotNull(result);
        Assert.Equal(value.Name, result.Name);
        Assert.Equal(value.Value, result.Value);
    }

    [Fact]
    public async Task SetAsync_WithNullValue_ShouldHandleGracefully()
    {
        var key = "null-key";
        TestData? value = null;
        var expiration = TimeSpan.FromMinutes(30);

        await _cacheService.SetAsync(key, value, null, expiration, null);

        var result = await _cacheService.GetAsync<TestData>(key);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_WithValidKey_ShouldRemoveFromCache()
    {
        var key = "test-key";
        var value = new TestData { Name = "Test", Value = 123 };
        
        await _cacheService.SetAsync(key, value, null, TimeSpan.FromMinutes(5), null);
        
        Assert.NotNull(await _cacheService.GetAsync<TestData>(key));

        await _cacheService.RemoveAsync(key);

        Assert.Null(await _cacheService.GetAsync<TestData>(key));
    }

    [Fact]
    public async Task ExistsAsync_WithExistingKey_ShouldReturnTrue()
    {
        var key = "existing-key";
        var value = new TestData { Name = "Test", Value = 123 };
        
        await _cacheService.SetAsync(key, value, null, TimeSpan.FromMinutes(5), null);

        var result = await _cacheService.ExistsAsync(key);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentKey_ShouldReturnFalse()
    {
        var key = "non-existent-key";

        var result = await _cacheService.ExistsAsync(key);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithEmptyString_ShouldReturnFalse()
    {
        var key = "empty-key";

        await _cache.SetStringAsync(key, "");

        var result = await _cacheService.ExistsAsync(key);

        Assert.False(result);
    }

    [Fact]
    public async Task SetAsync_WithAllExpirationOptions_ShouldHandleCorrectly()
    {
        var key = "test-key";
        var value = new TestData { Name = "Test", Value = 123 };
        var absoluteExpiration = DateTimeOffset.UtcNow.AddHours(1);
        var absoluteExpirationRelativeNow = TimeSpan.FromMinutes(30);
        var slidingExpiration = TimeSpan.FromMinutes(10);

        await _cacheService.SetAsync(key, value, absoluteExpiration, absoluteExpirationRelativeNow, slidingExpiration);

        var result = await _cacheService.GetAsync<TestData>(key);
        Assert.NotNull(result);
        Assert.Equal(value.Name, result.Name);
        Assert.Equal(value.Value, result.Value);
    }

    private class TestData
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
} 