
using Castle.Core.Logging;
using Mews.Caching.Builders;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Sdk;

namespace Mews.Caching.Tests.Builders
{
    public class CustomCacheBuilderTests
    {
        private readonly Mock<ILogger<ICustomCache>> _logger;
        private readonly Mock<IMemoryCache> _memoryCache;
        private readonly Mock<CustomCacheOptions> _customCacheOptions;


        public CustomCacheBuilderTests()
        {
            _logger = new Mock<ILogger<ICustomCache>>();
            _memoryCache = new Mock<IMemoryCache>();
            _customCacheOptions = new Mock<CustomCacheOptions>();
        }

        [Fact]
        public void When_OptionsNotSet_Then_Throws_NullException()
        {
            var cacheBuilder = CustomCacheBuilder.Start(_logger.Object);

            Assert.Throws<NullReferenceException>(() => cacheBuilder.Using(_memoryCache.Object).Build());
        }


        [Fact]
        public void Build_WithMemoryCache_ShouldReturnICustomCacheInstance()
        {
            var cacheBuilder = CustomCacheBuilder.Start(_logger.Object);

            var customCache = cacheBuilder.Using(_memoryCache.Object)
                .Using(_customCacheOptions.Object).Build();

            Assert.NotNull(customCache);
            Assert.IsAssignableFrom<ICustomCache>(customCache);
        }

    }
}
