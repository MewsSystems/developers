using ExchangeRateUpdater;
using ExchangeRateUpdater.API.CNB;
using ExchangeRateUpdater.API.CNB.Model.Responses;
using Moq;

namespace ExchangeRateUpdaterTest
{
    public class ExchangeRateProviderTest
    {
        private Mock<ICNBApiClient> _cnbApiClientMock;
        private ExchangeRateProvider _exchangeRateProvider;

        [SetUp]
        public void Setup()
        {
            _cnbApiClientMock = new Mock<ICNBApiClient>();
            _exchangeRateProvider = new ExchangeRateProvider(_cnbApiClientMock.Object);
        }

        [Test]
        public async Task GetExchangeRates_ShouldReturnRatesForSpecifiedCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var apiResponse = new ExRateDailyResponse
            {
                Rates = new List<ExRateDailyRest>
                {
                    new ExRateDailyRest { CurrencyCode = "USD", Rate = 25.0m, Amount = 1,  },
                    new ExRateDailyRest { CurrencyCode = "EUR", Rate = 27.0m, Amount = 1 },
                    new ExRateDailyRest { CurrencyCode = "GBP", Rate = 30.0m, Amount = 1 }
                }
            };

            _cnbApiClientMock.Setup(c => c.GetDailyRates(It.IsAny<string>(), It.IsAny<DateTime?>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "USD"), Is.True);
            Assert.That(result.Any(r => r.SourceCurrency.Code == "EUR"), Is.True);
        }

        [Test]
        public async Task GetExchangeRates_ShouldIgnoreUndefinedCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("XYZ")
            };

            var apiResponse = new ExRateDailyResponse
            {
                Rates = new List<ExRateDailyRest>
                {
                    new ExRateDailyRest { CurrencyCode = "USD", Rate = 25.0m, Amount = 1 }
                }
            };

            _cnbApiClientMock.Setup(c => c.GetDailyRates(It.IsAny<string>(), It.IsAny<DateTime?>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "USD"), Is.True);
            Assert.That(result.Any(r => r.SourceCurrency.Code == "XYZ"), Is.False);
        }

        [Test]
        public async Task GetExchangeRates_ShouldReturnEmptyWhenNoRatesDefined()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var apiResponse = new ExRateDailyResponse
            {
                Rates = new List<ExRateDailyRest>()
            };

            _cnbApiClientMock.Setup(c => c.GetDailyRates(It.IsAny<string>(), It.IsAny<DateTime?>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetExchangeRates_ShouldHandleNullRatesFromApi()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var apiResponse = new ExRateDailyResponse
            {
                Rates = null
            };

            _cnbApiClientMock.Setup(c => c.GetDailyRates(It.IsAny<string>(), It.IsAny<DateTime?>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetExchangeRates_ShouldThrowException_WhenApiClientFails()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD") };
            _cnbApiClientMock.Setup(c => c.GetDailyRates(It.IsAny<string>(), It.IsAny<DateTime?>())).ThrowsAsync(new Exception("API failure"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _exchangeRateProvider.GetExchangeRates(currencies));
        }
    }
}