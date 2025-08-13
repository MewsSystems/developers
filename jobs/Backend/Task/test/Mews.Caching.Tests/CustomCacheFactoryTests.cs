using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Mews.Caching.Tests
{
    public class CustomCacheFactoryTests
    {
        private readonly Mock<IMemoryCache> _memoryCache;
        private readonly Mock<IOptionsMonitor<CustomCacheOptions>> _optionsMonitor;
        private readonly Mock<ILogger<ICustomCache>> _logger;
        private ICustomCacheFactory _sut;

        public CustomCacheFactoryTests()
        {
            _memoryCache = new Mock<IMemoryCache>();
            _optionsMonitor = new Mock<IOptionsMonitor<CustomCacheOptions>>();
            _logger = new Mock<ILogger<ICustomCache>>();

            _sut = new CustomCacheFactory(_memoryCache.Object, _optionsMonitor.Object, _logger.Object);
        }

        [Fact]
        public void Ctor_When_Arguments_Are_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CustomCacheFactory(default, _optionsMonitor.Object, _logger.Object));
            Assert.Throws<ArgumentNullException>(() => new CustomCacheFactory(_memoryCache.Object, default, _logger.Object));
            Assert.Throws<ArgumentNullException>(() => new CustomCacheFactory(_memoryCache.Object, _optionsMonitor.Object, default));
        }


        [Theory]
        [InlineData("Test_Cache")]
        public void When_GetOrCreate_CacheFactory_Crate_NewOne(
            string testCache)
        {
            var options = new CustomCacheOptions()
            {
                Name = testCache
            };

            SetupCacheOptions(options);
            _sut = new CustomCacheFactory(_memoryCache.Object, _optionsMonitor.Object, _logger.Object);

            var actual = _sut.GetOrCreate(testCache);
            Assert.NotNull(actual);

            actual = _sut.GetOrCreate(testCache);
            Assert.NotNull(actual);
        }

        public void SetupCacheOptions(CustomCacheOptions customCacheOptions)
        {
            _optionsMonitor.Setup(x => x.Get(It.IsAny<string>())).Returns(customCacheOptions);
        }
    }
}
