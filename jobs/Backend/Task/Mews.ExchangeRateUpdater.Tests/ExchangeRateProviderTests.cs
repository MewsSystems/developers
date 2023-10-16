using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services;
using Moq;

namespace Mews.ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private Mock<IExchangeRateProviderService> _exchangeRateProviderServiceMock;

        private ExchangeRateProvider _sut;

        [SetUp]
        public void SetUp()
        {
            _exchangeRateProviderServiceMock = new Mock<IExchangeRateProviderService>();
            _exchangeRateProviderServiceMock.Setup(x => x.GetExchangeRates(It.IsAny<List<CurrencyDto>>())).ReturnsAsync(new List<ExchangeRateDto> { new ExchangeRateDto(new CurrencyDto("GBP"), new CurrencyDto("CZK"), 28.55M) });

            _sut = new ExchangeRateProvider(_exchangeRateProviderServiceMock.Object);
        }

        [Test]
        public async Task GetExchangeRates_Always_CallsGetExchangeRatesOnExchangeRateProviderServcice()
        {
            // Arrange
            var currencies = new List<CurrencyDto> { new CurrencyDto("GBP") };

            // Act
            await _sut.GetExchangeRates(currencies);

            // Assert
            _exchangeRateProviderServiceMock.Verify(x => x.GetExchangeRates(currencies), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_OnSuccess_ReturnsExchangeRateDtoCollection()
        {
            // Arrange
            var currencies = new List<CurrencyDto> { new CurrencyDto("GBP") };

            // Act
            var actual = await _sut.GetExchangeRates(currencies);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<IEnumerable<ExchangeRateDto>>(actual);
            Assert.That(actual.ToList()[0].SourceCurrency.Code, Is.EqualTo(currencies[0].Code));
            Assert.That(actual.ToList()[0].TargetCurrency.Code, Is.EqualTo("CZK"));
            Assert.That(actual.ToList()[0].Value, Is.EqualTo(28.55M));
        }
    }
}
