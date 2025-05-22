using ExchangeRateUpdater.Models;
using ExchangeRateUpdater;
using Moq;
using Xunit;

namespace ExchangeRateUpdaterTests.ExchangeRateProviderTests
{

    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsRatesFromService_WhenCurrenciesProvided()
        {
            // Arrange
            var currencies = new[]
            {
            new Currency("CZK"),
            new Currency("USD")
        };

            var expectedRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 22.5m)
        };

            var serviceMock = new Mock<IExchangeRateProviderService>();
            serviceMock.Setup(s => s.GetExchangeRateAsync(It.Is<IEnumerable<Currency>>(c => c.SequenceEqual(currencies))))
                       .ReturnsAsync(expectedRates);

            var provider = new ExchangeRateProvider(serviceMock.Object);

            // Act
            var result = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("USD", result.First().TargetCurrency.Code);
            serviceMock.Verify(s => s.GetExchangeRateAsync(It.IsAny<IEnumerable<Currency>>()), Times.Once);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenCurrencyListIsNull()
        {
            // Arrange
            var serviceMock = new Mock<IExchangeRateProviderService>();
            var provider = new ExchangeRateProvider(serviceMock.Object);

            // Act
            var result = await provider.GetExchangeRatesAsync(null);

            // Assert
            Assert.Empty(result);
            serviceMock.Verify(s => s.GetExchangeRateAsync(It.IsAny<IEnumerable<Currency>>()), Times.Never);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenCurrencyListIsEmpty()
        {
            // Arrange
            var serviceMock = new Mock<IExchangeRateProviderService>();
            var provider = new ExchangeRateProvider(serviceMock.Object);

            // Act
            var result = await provider.GetExchangeRatesAsync(new List<Currency>());

            // Assert
            Assert.Empty(result);
            serviceMock.Verify(s => s.GetExchangeRateAsync(It.IsAny<IEnumerable<Currency>>()), Times.Never);
        }
    }
}
