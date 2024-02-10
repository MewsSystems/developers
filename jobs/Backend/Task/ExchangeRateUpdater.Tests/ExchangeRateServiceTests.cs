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
        private Mock<ICzechNationalBankClient> _apiClientMock;

        [SetUp]
        public void Setup()
        {
            _apiClientMock = new Mock<ICzechNationalBankClient>();
            _apiClientMock.Setup(x => x.CnbapiExratesDailyAsync(It.IsAny<DateTimeOffset>(), It.IsAny<Lang>()))
            .ReturnsAsync(new ExRateDailyResponse
            {
                Rates = new List<ExRateDailyRest> { new ExRateDailyRest { CurrencyCode = "USD", Rate = 1.2 }, new ExRateDailyRest { CurrencyCode = "CAD", Rate = 1.6 } }
            });
        }

        [Test]
        public async Task GetExchangeRatesAsync_ValidInput_ReturnsExchangeRatesforProvidedCurrencies()
        {
            // Arrange
            IExchangeRateService exchangeRateService = new ExchangeRateService(_apiClientMock.Object);

            // Act
            IEnumerable<IExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<ICurrency> { new Currency("USD"), new Currency("CAD") });

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));

            IExchangeRate usdRate = result.First();
            IExchangeRate cadRate = result.Last();

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
            IExchangeRateService exchangeRateService = new ExchangeRateService(_apiClientMock.Object);

            // Act
            IEnumerable<IExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<ICurrency> { new Currency("CZK") });

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetExchangeRatesAsync_ApiReturnsEmpty_ReturnsEmptyList()
        {
            // Arrange
            Mock<ICzechNationalBankClient> apiClientMock = new Mock<ICzechNationalBankClient>();
            apiClientMock.Setup(x => x.CnbapiExratesDailyAsync(It.IsAny<DateTimeOffset>(), It.IsAny<Lang>()))
                .ReturnsAsync(new ExRateDailyResponse
                {
                    Rates = new List<ExRateDailyRest>()
                });

            IExchangeRateService exchangeRateService = new ExchangeRateService(apiClientMock.Object);

            // Act
            IEnumerable<IExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<ICurrency> { new Currency("EUR") });

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }


        [Test]
        public void GetExchangeRatesAsync_NullCurrencies_ThrowsArgumentNullException()
        {
            // Arrange
            IExchangeRateService exchangeRateService = new ExchangeRateService(_apiClientMock.Object);

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
            IExchangeRateService exchangeRateService = new ExchangeRateService(_apiClientMock.Object);

            //Act
            IEnumerable<IExchangeRate> result = await exchangeRateService.GetExchangeRatesAsync("CZK", new List<ICurrency>());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetExchangeRatesAsync_ApiException_ThrowsApiException()
        {
            // Arrange
            Mock<ICzechNationalBankClient> apiClientMock = new Mock<ICzechNationalBankClient>();
            apiClientMock.Setup(x => x.CnbapiExratesDailyAsync(It.IsAny<DateTimeOffset>(), It.IsAny<Lang>()))
                .ThrowsAsync(new ApiException("Api Exception", 500, "", null, null));

            IExchangeRateService exchangeRateService = new ExchangeRateService(apiClientMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ApiException>(async () =>
            {
                await exchangeRateService.GetExchangeRatesAsync("CZK", new List<ICurrency> { new Currency("USD") });
            });
        }
    }
}
