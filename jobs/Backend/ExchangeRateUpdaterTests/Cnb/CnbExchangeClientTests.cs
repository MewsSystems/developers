using ExchangeRateUpdater.BusinessLogic.Interfaces;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants;
using ExchangeRateUpdater.Clients.Cnb.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ExchangeRateUpdaterTests.Cnb
{
    public class CnbExchangeClientTests
    {
        [Fact]
        public void WhenGetExchangeRateTxtAsync_WithInvalidParam_ShouldThrowException()
        {
            var mockedConfig = new Mock<IConfiguration>();
            var mockedHttpfactory = new Mock<IHttpClientFactory>();

            Assert.Throws<ArgumentNullException>(() => new CnbExchangeClient(mockedConfig.Object, mockedHttpfactory.Object).GetExchangeRateTxtAsync(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void WhenGetFxExchangeRateTxtAsync_WithInvalidParam_ShouldThrowException()
        {
            var mockedConfig = new Mock<IConfiguration>();
            var mockedHttpfactory = new Mock<IHttpClientFactory>();

            Assert.Throws<ArgumentNullException>(() => new CnbExchangeClient(mockedConfig.Object, mockedHttpfactory.Object).GetFxExchangeRateTxtAsync(null).GetAwaiter().GetResult());
        }
    }
}