using Mews.Caching.Common;
using Mews.Caching.InMemory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Mews.Caching.Tests
{
    public class CustomCacheBaseTests
    {
        private readonly CustomCacheOptions options;
        private readonly Mock<ILogger<ICustomCache>> _mockLogger;
        private readonly IMemoryCache _memoryCache;
        private TestInMemoryCustomCache _sut;


        public CustomCacheBaseTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();

            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            options = new CustomCacheOptions(); ;
            _mockLogger = new Mock<ILogger<ICustomCache>>();
            _sut = new TestInMemoryCustomCache(_memoryCache, options, _mockLogger.Object);
        }

        [Theory]
        [InlineData("Test_key")]
        public async Task When_GetCache_Obtain_CachedItem(
            string testKey)
        {
            var result = _sut.Get<string>(testKey);
            Assert.False(result.HasValue);
        }


        [Theory]
        [InlineData("Test_key")]
        public async Task When_GetAsyncCache_Obtain_CachedItem(
            string testKey)
        {
            var result = await _sut.GetAsync<string>(testKey);
            Assert.False(result.HasValue);
        }


        [Theory]
        [InlineData("Test_key", "test_value")]
        public void When_SetCache_Obtain_CachedItem(
       string testKey,
       string value)
        {
            _sut.Set(testKey, value);
            var result = _sut.Get<string>(testKey);
            Assert.True(result.HasValue);
            Assert.Equal(result.Value, value);
        }

        [Theory]
        [InlineData("Test_key", "test_value")]
        public async Task When_SetAsyncCache_Obtain_CachedItem(
         string testKey,
         string value)
        {
            await _sut.SetAsync(testKey, value);
            var result = await _sut.GetAsync<string>(testKey);
            Assert.True(result.HasValue);
            Assert.Equal(result.Value, value);
        }


        [Theory]
        [InlineData("Test_key", "test_value", "10:00:00")]
        public async Task When_SetCache_WithAbsoluteExpirationRelativeToNow_Obtain_CachedItem(
         string testKey,
         string value,
         string expiration)
        {
            TimeSpan absoluteExpirationRelativeToNow = TimeSpan.Parse(expiration);
            _sut.Set(testKey, value, absoluteExpirationRelativeToNow);
            var result = await _sut.GetAsync<string>(testKey);
            Assert.True(result.HasValue);
            Assert.Equal(result.Value, value);
        }


        [Theory]
        [InlineData("Test_key", "test_value")]
        public async Task When_GetOrAddCache_Obtain_CachedItem(
           string testKey,
           string value)
        {

            var result = await _sut.GetAsync<string>(testKey);
            Assert.False(result.HasValue);

            var actual = _sut.GetOrAdd(testKey, () => value);

            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }


        [Theory]
        [InlineData("Test_key", "test_value")]
        public async Task When_GetOrAddAsyncCache_Obtain_CachedItem(
           string testKey,
           string value)
        {

            var result = await _sut.GetAsync<string>(testKey);
            Assert.False(result.HasValue);

            var actual = await _sut.GetOrAddAsync(testKey, async () => value);


            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);

            actual = await _sut.GetAsync<string>(testKey);
            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }

        [Theory]
        [InlineData("Test_key", "test_value", "00:00:10")]
        public async Task When_GetOrAddCache_WithCancellationToken_Obtain_CachedItem(
          string testKey,
          string value,
          string expiration)
        {
            var result = await _sut.GetAsync<string>(testKey);
            Assert.False(result.HasValue);

            TimeSpan absoluteExpirationRelativeToNow = TimeSpan.Parse(expiration);
            var actual = _sut.GetOrAdd(testKey, () => value, CancellationToken.None);


            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);

            actual = await _sut.GetAsync<string>(testKey);
            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }



        [Theory]
        [InlineData("Test_key", "test_value", "00:00:10")]
        public async Task When_GetOrAddAsyncCache_WithabsoluteExpirationRelativeToNow_WithCancellationToken_Obtain_CachedItem(
      string testKey,
      string value,
      string expiration)
        {
            var result = await _sut.GetAsync<string>(testKey);
            Assert.False(result.HasValue);

            TimeSpan absoluteExpirationRelativeToNow = TimeSpan.Parse(expiration);
            var actual = await _sut.GetOrAddAsync(testKey, async () => await Task.FromResult(value), absoluteExpirationRelativeToNow);


            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);

            actual = await _sut.GetAsync<string>(testKey);
            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }



        [Theory]
        [InlineData("Test_key", "test_value", "00:00:10")]
        public async Task When_GetOrAddAsyncCache_WithExpirationDate_Obtain_CachedItem(
           string testKey,
           string value,
           string expiration)
        {

            var result = await _sut.GetAsync<string>(testKey);
            Assert.False(result.HasValue);

            TimeSpan absoluteExpirationRelativeToNow = TimeSpan.Parse(expiration);
            var actual = await _sut.GetOrAddAsync(testKey, async () => value);


            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);

            actual = await _sut.GetAsync<string>(testKey);
            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }

        [Theory]
        [InlineData("Test_key", "test_value", "00:00:10")]
        public async Task When_GetOrAddAsyncCache_WithExpirationDate_WithOptions_Obtain_CachedItem(
           string testKey,
           string value,
           string expiration)
        {
            var testOptions = new CustomCacheOptions()
            {
                DefaultAbsoluteExpirationRelativeToNow = TimeSpan.Parse(expiration)
            };
            _sut = new TestInMemoryCustomCache(_memoryCache, testOptions, _mockLogger.Object);

            var actual = await _sut.GetOrAddAsync(testKey, async () => value);


            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);

            actual = await _sut.GetAsync<string>(testKey);
            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }


        [Theory]
        [InlineData("Test_key", "test_value", "00:00:10")]
        public async Task When_GetOrAddAsyncCache_WithExpirationDate_WithCancellationToken_Obtain_CachedItem(
        string testKey,
        string value,
        string expiration)
        {
            var testOptions = new CustomCacheOptions()
            {
                DefaultAbsoluteExpirationRelativeToNow = TimeSpan.Parse(expiration)
            };
            _sut = new TestInMemoryCustomCache(_memoryCache, testOptions, _mockLogger.Object);

            var actual = await _sut.GetOrAddAsync(testKey, async () => value, CancellationToken.None);


            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);

            actual = await _sut.GetAsync<string>(testKey);
            Assert.True(actual.HasValue);
            Assert.Equal(actual.Value, value);
        }
    }


    internal class TestInMemoryCustomCache : InMemoryCustomCache
    {


        public TestInMemoryCustomCache(
           IMemoryCache memoryCache,
           CustomCacheOptions options,
           ILogger<ICustomCache> logger) : base(memoryCache, options, logger) { }

    }
}