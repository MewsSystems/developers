using ExchangeRateUpdater.Services.Cache;

namespace ExchangeRateUpdater.Tests;

public class FileCacheServiceTests : IDisposable
{
    private readonly string _tempFilePath;
    private readonly FileCacheService _fileCacheService;

    public FileCacheServiceTests()
    {
        _tempFilePath = Path.Combine(Path.GetTempPath(), $"testcache_{Guid.NewGuid()}.json");
        _fileCacheService = new FileCacheService(_tempFilePath);
    }

    [Fact]
    public async Task SetAndGetAsync_StoresAndRetrievesValue()
    {
        var key = "test-key";
        var testObject = new TestCacheObject { Value = "Hello, World!" };
        var ttl = TimeSpan.FromMinutes(5);

        await _fileCacheService.SetAsync(key, testObject, ttl);

        var cached = await _fileCacheService.GetAsync<TestCacheObject>(key);

        Assert.NotNull(cached);
        Assert.Equal(testObject.Value, cached.Value);
    }

    [Fact]
    public async Task GetAsync_ReturnsNull_WhenKeyDoesNotExist()
    {
        var cached = await _fileCacheService.GetAsync<TestCacheObject>("non-existent-key");
        Assert.Null(cached);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }

    private class TestCacheObject
    {
        public string Value { get; set; }
    }
}
