using CzechNationalBankClient;
using CzechNationalBankClient.Model;
using ExchangeRateProvider;
using ExchangeRateProvider.Objects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateTest
{
    public class ExchangeRateProviderServiceTests
    {
        private Mock<ICurrencyExchangeRateClient> _currencyExchangeRateClientMock;
        private Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private Mock<ILogger<IExchangeRateProviderService>> _loggerMock;
        private IExchangeRateProviderService _exchangeRateProviderService;

        [SetUp]
        public void Setup()
        {
            _currencyExchangeRateClientMock = new Mock<ICurrencyExchangeRateClient>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _loggerMock = new Mock<ILogger<IExchangeRateProviderService>>();
            _exchangeRateProviderService = new ExchangeRateProviderService(_currencyExchangeRateClientMock.Object,
                _memoryCacheServiceMock.Object, _loggerMock.Object);
        }

        [TestCase("USD", 1)]
        [TestCase("", 0)]
        [TestCase("USDD", 0)]
        [TestCase("XYZ", 0)]
        public async Task RetrieveRatesFromCache(string currency, int expected)
        {
            _memoryCacheServiceMock.Setup(m => m.GetCachedRatesValue(It.IsAny<string>()))
            .Returns(new[] { new CnbExchangeRate { CurrencyCode = "USD", Rate = 1.5m }, new CnbExchangeRate { CurrencyCode = "JPY", Rate = 2m } } );

            var result = await _exchangeRateProviderService.RetrieveExchangeRatesAsync(new[]
            {
                new Currency(currency)
            }, DateTime.Today);

            result.Should().NotBeNull();
            result.Should().HaveCount(expected);
            _memoryCacheServiceMock.Verify(m => m.GetCachedRatesValue(It.IsAny<string>()), Times.Exactly(2));
            _memoryCacheServiceMock.Verify(m => m.SetCachedRates(It.IsAny<string>(), It.IsAny<IEnumerable<CnbExchangeRate>>()), Times.Never);
            _currencyExchangeRateClientMock.Verify(m => m.GetCurrencyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _currencyExchangeRateClientMock.Verify(m => m.GetOtherCurrencyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public async Task RetrieveRatesFromSource()
        {
            _memoryCacheServiceMock.Setup(m => m.GetCachedRatesValue(It.IsAny<string>()))
            .Returns(default(IEnumerable<CnbExchangeRate>));
            _currencyExchangeRateClientMock.Setup(m => m.GetCurrencyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new[] { new CnbExchangeRate { CurrencyCode = "USD", Rate = 1.5m }, new CnbExchangeRate { CurrencyCode = "JPY", Rate = 2m } });
            _currencyExchangeRateClientMock.Setup(m => m.GetOtherCurrencyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new[] { new CnbExchangeRate { CurrencyCode = "THB", Rate = 3m } });

            var result = await _exchangeRateProviderService.RetrieveExchangeRatesAsync(new[]
            {
                new Currency("THB")
            }, DateTime.Today);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            _memoryCacheServiceMock.Verify(m => m.GetCachedRatesValue(It.IsAny<string>()), Times.Exactly(2));
            _memoryCacheServiceMock.Verify(m => m.SetCachedRates(It.IsAny<string>(), It.IsAny<IEnumerable<CnbExchangeRate>>()), Times.Exactly(2));
            _currencyExchangeRateClientMock.Verify(m => m.GetCurrencyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _currencyExchangeRateClientMock.Verify(m => m.GetOtherCurrencyExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}