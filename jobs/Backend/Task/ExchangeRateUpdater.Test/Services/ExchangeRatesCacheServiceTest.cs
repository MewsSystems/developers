using ExchangeRateUpdater.Implementation.Services;
using ExchangeRateUpdater.Interface.Services;
using ExchangeRateUpdater.Test.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.Services
{
    [TestClass]
    public class ExchangeRatesCacheServiceTest
    {
        private Mock<ILogger<IExchangeRatesCacheService>> _loggerMock;
        private readonly Mock<ICnbApiService> _cnbApiServiceMock;
        private readonly IMemoryCache _cache;

        public ExchangeRatesCacheServiceTest()
        {
            _loggerMock = new Mock<ILogger<IExchangeRatesCacheService>>();
            _cnbApiServiceMock = new Mock<ICnbApiService>();
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        [TestMethod]
        public async Task GetOrCreateExchangeRatesAsync()
        {
            _cnbApiServiceMock.Setup(c => c.GetExchangeRates()).ReturnsAsync(TestObjects.ExchangeRateEntityList);

            var service = new ExchangeRatesCacheService(_loggerMock.Object, _cnbApiServiceMock.Object, _cache);

            var result = await service.GetOrCreateExchangeRatesAsync();

            Assert.IsNotNull(result);
            _cnbApiServiceMock.Verify(x => x.GetExchangeRates(), Times.Once);

            result = await service.GetOrCreateExchangeRatesAsync();

            //Check if _cnbApiServiceMock.GetExchangeRates() was called again
            Assert.IsNotNull(result);
            _cnbApiServiceMock.Verify(x => x.GetExchangeRates(), Times.Once);
        }
    }
}
