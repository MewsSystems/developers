using ExchangeRateUpdater.Cache;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Moq;

namespace ExchangeRateUpdaterTests
{
    [TestFixture]
    public class ExchangeRateProviderTest
    {
        private Mock<IExchangeService> _exchangeServiceMock;
        private Mock<ICacheService<string, List<ExchangeRateRecord>>> _cacheServiceMock;

        [SetUp]
        public void Setup()
        {
            _exchangeServiceMock = new();
            _cacheServiceMock = new();
        }

        [Test]
        public async Task GetExchangeRates_MultipleCurrencies_ReturnsNotNullAndNotEmpty()
        {
            //Arragne
            var currencies = new List<Currency>
            {
                MockData.AustralianCurrency,
                MockData.BrasilianCurrency
            };

            List<ExchangeRateRecord> nullCachedValue = null;

            _cacheServiceMock
                .Setup(cs => cs.Get(It.IsAny<string>()))
                .Returns(nullCachedValue);
            _cacheServiceMock
                .Setup(cs => cs.Add(It.IsAny<string>(), It.IsAny<List<ExchangeRateRecord>>(), It.IsAny<TimeSpan>()));

            _exchangeServiceMock
                .Setup(es => es.GetExchangeRatesAsync())
                .ReturnsAsync(MockData.ValidExchangeRateRecordList);

            var exchangeRateProvider = new ExchangeRateProvider(_cacheServiceMock.Object, _exchangeServiceMock.Object);

            //Act
            var result = await exchangeRateProvider.GetExchangeRatesAsync(currencies, MockData.CzechCurrency);

            //Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            _cacheServiceMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
            _cacheServiceMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<List<ExchangeRateRecord>>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_InvalidCurrencies_ReturnsNotNullAndEmpty()
        {
            //Arragne
            var invalidCurrencies = new List<Currency>
            {
               new Currency("XYZ")
            };

            List<ExchangeRateRecord> nullCachedValue = null;

            _cacheServiceMock
                .Setup(cs => cs.Get(It.IsAny<string>()))
                .Returns(nullCachedValue);
            _cacheServiceMock
                .Setup(cs => cs.Add(It.IsAny<string>(), It.IsAny<List<ExchangeRateRecord>>(), It.IsAny<TimeSpan>()));

            _exchangeServiceMock
                .Setup(es => es.GetExchangeRatesAsync())
                .ReturnsAsync(MockData.ValidExchangeRateRecordList);

            var exchangeRateProvider = new ExchangeRateProvider(_cacheServiceMock.Object, _exchangeServiceMock.Object);

            //Act
            var result = await exchangeRateProvider.GetExchangeRatesAsync(invalidCurrencies, MockData.CzechCurrency);

            //Assert
            Assert.NotNull(result);
            Assert.IsEmpty(result);
            _cacheServiceMock.Verify(c => c.Get(It.IsAny<string>()), Times.Once);
            _cacheServiceMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<List<ExchangeRateRecord>>(), It.IsAny<TimeSpan>()), Times.Once);
        }


        [Test]
        public void GetExchangeRates_NullCurrencies_ThrowsArgumentNullException()
        {
            //Arragne
            List<Currency> currencies = null;

            var exchangeRateProvider = new ExchangeRateProvider(_cacheServiceMock.Object, _exchangeServiceMock.Object);

            //Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await exchangeRateProvider.GetExchangeRatesAsync(currencies, MockData.CzechCurrency));
        }
    }
}
