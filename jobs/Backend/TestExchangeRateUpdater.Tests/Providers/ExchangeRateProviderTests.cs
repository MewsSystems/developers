using Castle.Core.Logging;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Logging;


namespace ExchangeRateUpdater.Tests.Providers
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private Mock<IExchangeRateService> _exchangeRateServiceMock;
        private Mock<ILogger<ExchangeRateProvider>> _loggerMock;
        private ExchangeRateProvider _exchangeRateProvider;

        [SetUp]
        public void SetUp()
        {
            _exchangeRateServiceMock = new Mock<IExchangeRateService>();
            _exchangeRateServiceMock.Setup(_exchangeRateServiceMock => _exchangeRateServiceMock.GetExchangeRatesAsync())
                .ReturnsAsync(new ExchangeRatesDTO
                {
                    Rates = new List<ExchangeRateDTO>
                    {
                        new ()
                        {
                            CurrencyCode = "ABC",
                            Rate = 1.0M,
                            Amount = 1
                        },
                        new ()
                        {
                            CurrencyCode = "DEF",
                            Rate = 30M,
                            Amount = 10
                        },
                        new ()
                        {
                            CurrencyCode = "GHI",
                            Rate = 20M,
                            Amount = 100
                        }
                    }
                });
            _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
            _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetExchangeRates_ShouldReturnCorrectExchangeRates()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new("ABC"),
                new("DEF"),
                new("GHI")
            };
            var expected = new List<ExchangeRate>
            {
                new(new Currency("ABC"), new Currency("CZK"), 1M),
                new(new Currency("DEF"), new Currency("CZK"), 3M),
                new(new Currency("GHI"), new Currency("CZK"), 0.2M)
            };

            // Act
            var actual = _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetExchangeRates_ShouldReturnAnEmptyList_WhenNoMatchingCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new("RST"),
                new("UVW"),
                new("XYZ")
            };

            // Act
            var actual = _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            actual.Should().BeEmpty();
        }

        [Test]
        public void GetExchangeRates_ShouldLogInformation_WhenNoMatchingCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new("JKL"),
                new("MNO"),
                new("PQR")
            };

            // Act
            _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No matching rates found for chosen currency codes")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
