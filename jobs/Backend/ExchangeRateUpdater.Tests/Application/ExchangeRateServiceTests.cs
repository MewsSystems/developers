using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using Moq;

namespace ExchangeRateUpdater.Tests.Application
{
    public class ExchangeRateServiceTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_DelegatesToProvider_AndReturnsResult()
        {
            // Arrange
            var mockProvider = new Mock<IExchangeRateProvider>();
            var sampleRates = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m)
            };
            mockProvider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                        .ReturnsAsync(sampleRates);

            var service = new ExchangeRateService(mockProvider.Object);
            var currencies = new List<Currency> { new Currency("USD") };

            // Act
            var result = await service.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result.First().SourceCurrency.Code);
            mockProvider.Verify(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()), Times.Once);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_EmptyCurrencyList_ReturnsEmptyResult()
        {
            // Arrange
            var mockProvider = new Mock<IExchangeRateProvider>();
            mockProvider.Setup(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                        .ReturnsAsync(new List<ExchangeRate>());

            var service = new ExchangeRateService(mockProvider.Object);
            var currencies = new List<Currency>();

            // Act
            var result = await service.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Empty(result);
            mockProvider.Verify(p => p.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()), Times.Once);
        }

    }
}
