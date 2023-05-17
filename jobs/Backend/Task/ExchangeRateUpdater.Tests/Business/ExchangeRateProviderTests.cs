using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Business
{
    public class ExchangeRateProviderTests
    {
        private Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private Mock<IForeignExchangeService> _mockForeignExchangeService;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockForeignExchangeService = new Mock<IForeignExchangeService>();
        }

        [Test]
        public async Task GetExchangeRatesAsync_CanNotFetchFromApi_ReturnsNull()
        {
            // Arrange
            _mockForeignExchangeService.Setup(x => x.GetLiveRatesAsync())
                .ReturnsAsync((List<ThirdPartyExchangeRate>)null);

            // Act
            var service = new ExchangeRateProvider(_mockLogger.Object, _mockForeignExchangeService.Object);
            var result = await service.GetExchangeRatesAsync();

            // Assert
            Assert.That(result, Is.Null);
            _mockForeignExchangeService.Verify(x => x.GetLiveRatesAsync(), Times.Once);
        }

        [Test]
        public async Task GetExchangeRatesAsync_CanFetchFromApi_ReturnsList()
        {
            // Arrange 
            var thirdPartyRates = GetThirdPartyExchangeRates();
            _mockForeignExchangeService.Setup(x => x.GetLiveRatesAsync())
                .ReturnsAsync(thirdPartyRates);

            // Act
            var service = new ExchangeRateProvider(_mockLogger.Object, _mockForeignExchangeService.Object);
            var result = await service.GetExchangeRatesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<List<ExchangeRate>>());
            Assert.That(result.Count, Is.EqualTo(thirdPartyRates.Count));
            _mockForeignExchangeService.Verify(x => x.GetLiveRatesAsync(), Times.Once);
        }

        #region Test data helpers

        public List<ThirdPartyExchangeRate> GetThirdPartyExchangeRates()
        {
            return new List<ThirdPartyExchangeRate>
            {
                new ThirdPartyExchangeRate {Country = "Australia", Currency = "dollar", Amount = 1, CurrencyCode = "AUD", Rate = 14.529M },
                new ThirdPartyExchangeRate {Country = "EMU", Currency = "euro", Amount = 1, CurrencyCode = "EUR", Rate = 23.635M },
                new ThirdPartyExchangeRate {Country = "United Kingdom", Currency = "pound", Amount = 1, CurrencyCode = "GBP", Rate = 27.203M }
            };
        }

        #endregion
    }
}
