using ExchangeRateUpdater;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.HttpUtils;
using Moq;

namespace UnitTests
{
    public class ExchangeRateProviderTests
    {
        private Mock<ICnbClient> _cnbClient;
        private ExchangeRateProvider _provider;

        [SetUp]
        public void Setup()
        {
            var exchangeRates = new List<ExchangeRate>() { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.6m) };
            _cnbClient = new Mock<ICnbClient>();
            _cnbClient.Setup(client => client.GetCurrentExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>())).Returns(Task.FromResult(exchangeRates.AsEnumerable()));
            _provider = new ExchangeRateProvider(_cnbClient.Object);
        }

        [Test]
        public void GetExchangeRates_CurrenciesIsNull_ReturnsEmptyList()
        {
            Assert.ThrowsAsync<ExchangeRatesException>(() => _provider.GetExchangeRates(null));
        }

        [Test]
        public async Task GetExchangeRates_CurrenciesAreEmpty_ReturnsEmptyList()
        {
            var currencies = new List<Currency>();
            var exchangeRates = await _provider.GetExchangeRates(currencies);
            Assert.IsNotNull(exchangeRates);
            Assert.IsEmpty(exchangeRates);
            _cnbClient.Verify(ms => ms.GetCurrentExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()), Times.Exactly(0));
        }

        [Test]
        public async Task GetExchangeRates_CurrenciesAreProvided_ReturnsNonEmptyList()
        {
            var currencies = new List<Currency>() { new Currency("USD")};
            var exchangeRates = await _provider.GetExchangeRates(currencies);
            Assert.IsNotNull(exchangeRates);
            Assert.IsNotEmpty(exchangeRates);
            _cnbClient.Verify(ms => ms.GetCurrentExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()), Times.Exactly(1));
        }
    }
}
