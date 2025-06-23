using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRateApiClientFactoryTests
    {
        [Fact]
        public void CreateExchangeRateApiClient_ReturnsCorrectExchangeRateApiClient()
        {
            // Arrange
            var factory = CreateExchangeRateApiClientFactory();

            // Act
            var exchangeRateApiClient = factory.CreateExchangeRateApiClient(WellKnownCurrencyCodes.CZK);

            // Assert
            Assert.NotNull(exchangeRateApiClient);
            Assert.IsType<CzechNationalBankExchangeRateApiClient>(exchangeRateApiClient);
        }

        [Fact]
        public void CreateExchangeRateApiClient_IfCurrencyCodeUnknown_ThrowsException()
        {
            // Arrange
            var factory = CreateExchangeRateApiClientFactory();

            // Act
            var exception = Assert.Throws<NotImplementedException>(
                () => factory.CreateExchangeRateApiClient("INVALID"));

            // Assert
            Assert.Equal("Exchange rate API client not implemented for currency INVALID", exception.Message);
        }

        private static ExchangeRateApiClientFactory CreateExchangeRateApiClientFactory()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns<HttpClient>(null);

            var loggerFactoryMock = new Mock<ILoggerFactory>();            

            return new(httpClientFactoryMock.Object, loggerFactoryMock.Object);
        }
    }
}
