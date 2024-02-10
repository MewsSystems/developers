using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateServiceTests
    {
        private Mock<IExchangeRateProxy> _proxyMock;

        [SetUp]
        public void Setup()
        {
            _proxyMock = new Mock<IExchangeRateProxy>();
                
            _proxyMock.Setup(x => x.GetCurrencyRatesAsync(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new List<CurrencyRate>
                {
                    new CurrencyRate { CurrencyCode = "USD", Rate = 1.2m },
                    new CurrencyRate { CurrencyCode = "CAD", Rate = 1.6m }
                });
        }

        [Test]
        public async Task GetExchangeRatesAsync_ValidInput_ReturnsExchangeRatesforProvidedCurrencies()
        {
            // Arrange
            IExchangeRateService exchangeRateService = new ExchangeRateService(_proxyMock.Object);

            // Act
            IEnumerable<ExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<string> { "USD", "CAD" });

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            ExchangeRate usdRate = result.First();
            ExchangeRate cadRate = result.Last();

            Assert.Multiple(() =>
            {
                Assert.That(usdRate.SourceCurrency.Code, Is.EqualTo("CZK"));
                Assert.That(usdRate.TargetCurrency.Code, Is.EqualTo("USD"));
                Assert.That(usdRate.Value, Is.EqualTo(0.83));

                Assert.That(cadRate.SourceCurrency.Code, Is.EqualTo("CZK"));
                Assert.That(cadRate.TargetCurrency.Code, Is.EqualTo("CAD"));
                Assert.That(cadRate.Value, Is.EqualTo(0.62));
            });
        }

        [Test]
        public async Task GetExchangeRatesAsync_SourceCurrencyCodeInCurrencies_DoesNotReturnSourceExchangeRate()
        {
            // Arrange
            IExchangeRateService exchangeRateService = new ExchangeRateService(_proxyMock.Object);

            // Act
            IEnumerable<ExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<string> { "CZK" });

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetExchangeRatesAsync_ApiReturnsEmpty_ReturnsEmptyList()
        {
            // Arrange
            Mock<IExchangeRateProxy> proxyMock = new Mock<IExchangeRateProxy>();

            proxyMock.Setup(x => x.GetCurrencyRatesAsync(It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new List<CurrencyRate>());

            IExchangeRateService exchangeRateService = new ExchangeRateService(proxyMock.Object);

            // Act
            IEnumerable<ExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<string> { "EUR" });

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }


        [Test]
        public void GetExchangeRatesAsync_NullCurrencies_ThrowsArgumentNullException()
        {
            // Arrange
            IExchangeRateService exchangeRateService = new ExchangeRateService(_proxyMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await exchangeRateService.GetExchangeRatesAsync("CZK", null);
            });
        }

        [Test]
        public async Task GetExchangeRatesAsync_EmptyCurrencies_ReturnsEmptyList()
        {
            // Arrange
            IExchangeRateService exchangeRateService = new ExchangeRateService(_proxyMock.Object);

            //Act
            IEnumerable<ExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<string>());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetExchangeRatesAsync_ApiException_ThrowsApiException()
        {
            // Arrange
            Mock<IExchangeRateProxy> proxyMock = new Mock<IExchangeRateProxy>();

            proxyMock.Setup(x => x.GetCurrencyRatesAsync(It.IsAny<DateTimeOffset>()))
                .ThrowsAsync(new ApiException("Api Exception", 500, "", null, null));


            IExchangeRateService exchangeRateService = new ExchangeRateService(proxyMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ApiException>(async () =>
            {
                await exchangeRateService.GetExchangeRatesAsync("CZK", new List<string> { "USD" });
            });
        }
    }
}
