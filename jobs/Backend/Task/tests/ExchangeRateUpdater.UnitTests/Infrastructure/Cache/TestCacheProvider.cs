using ExchangeRateUpdater.Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Infrastructure.Cache;

public class TestCacheProvider
{
    private class TestState
    {
        public Mock<IMemoryCache> Cache { get; }
        public Mock<ILogger<CacheProvider>> Logger { get; }
        public CacheProvider Subject { get; }

        public TestState()
        {
            Cache = new Mock<IMemoryCache>();
            Logger = new Mock<ILogger<CacheProvider>>();
            Subject = new CacheProvider(Cache.Object, Logger.Object);
        }
    }

    [Fact]
    public void WhenKeyExistsInCache_ThenReturnCachedValue()
    {
        TestState state = new TestState();
        string key = "testKey";
        string expectedValue = "cachedValue";
        object cacheEntry = expectedValue;

        state.Cache.Setup(c => c.TryGetValue(key, out cacheEntry)).Returns(true);

        bool result = state.Subject.TryGetCache<string>(key, out string? actualValue);

        Assert.True(result);
        Assert.Equal(expectedValue, actualValue);

        state.Cache.Verify(c => c.TryGetValue(key, out cacheEntry), Times.Once);
    }

    [Fact]
    public void WhenKeyDoesNotExistInCache_ThenReturnFalse()
    {
        TestState state = new TestState();
        string key = "nonExistentKey";
        object cacheEntry = null;

        state.Cache.Setup(c => c.TryGetValue(key, out cacheEntry)).Returns(false);

        bool result = state.Subject.TryGetCache<string>(key, out string? actualValue);

        Assert.False(result);
        Assert.Null(actualValue);

        state.Cache.Verify(c => c.TryGetValue(key, out cacheEntry), Times.Once);
    }

    [Fact]
    public void WhenTryGetCacheThrowsException_ThenReturnFalseAndLogError()
    {
        TestState state = new TestState();
        string key = "testKey";
        object cacheEntry = null;

        state.Cache.Setup(c => c.TryGetValue(key, out cacheEntry)).Throws(new Exception("Cache error"));

        bool result = state.Subject.TryGetCache<string>(key, out string? actualValue);

        Assert.False(result);
        Assert.Null(actualValue);

        state.Logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error retrieving cache")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once);
    }

    [Fact]
    public void WhenKeyDoesNotExistInCache_ThenStoreValueAndReturnTrue()
    {
        TestState state = new TestState();
        string key = "testKey";
        string value = "newValue";

        state.Cache.Setup(c => c.TryGetValue(key, out It.Ref<object>.IsAny)).Returns(false);
        state.Cache.Setup(c => c.CreateEntry(key))
            .Returns(new Mock<ICacheEntry>().Object);

        bool result = state.Subject.TrySetCache(key, value, 60);

        Assert.True(result);
        state.Cache.Verify(c => c.CreateEntry(key), Times.Once);
    }

    [Fact]
    public void WhenKeyExistInCache_ThenReturnsFalse_AndNotStoreInCache()
    {
        TestState state = new TestState();
        string key = "testKey";
        string value = "newValue";

        state.Cache.Setup(c => c.TryGetValue(key, out It.Ref<object>.IsAny)).Returns(true);

        bool result = state.Subject.TrySetCache(key, value, 60);

        Assert.False(result);
        state.Cache.Verify(c => c.CreateEntry(key), Times.Never);
    }
    
    [Fact]
    public void WhenTrySetCacheThrowsException_ThenReturnFalseAndLogError()
    {
        TestState state = new TestState();
        string key = "testKey";
        string value = "newValue";

        state.Cache.Setup(c => c.TryGetValue(key, out It.Ref<object>.IsAny)).Returns(false);
        state.Cache.Setup(c => c.CreateEntry(key))
            .Throws(new Exception());

        bool result = state.Subject.TrySetCache(key, value, 60);

        Assert.False(result);
        
        state.Logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error setting cache")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once);
    }
    
}