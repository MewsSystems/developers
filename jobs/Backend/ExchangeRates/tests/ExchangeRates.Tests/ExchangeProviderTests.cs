using CNB.Client;
using ExchangeRates.App.Caching;
using ExchangeRates.App.Provider;
using ExchangeRates.Domain;
using NodaTime;
using NSubstitute;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeProviderTests
    {
        private readonly IBankClient _clientMock = Substitute.For<IBankClient>();
        private readonly ICacheService _cacheMock = Substitute.For<ICacheService>();
        private readonly IClock _clock = Substitute.For<IClock>();

        [Fact]
        public async Task GetExchangeRates_ReturnsRatesFromCache_WhenAvailable()
        {
            // Arrange
            var currencies = new List<Currency> { new("USD") };
            var exchangeRate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 10);
            _cacheMock.GetCachedData<ExchangeRate>("USD/CZK").Returns(exchangeRate);

            var sut = new ExchangeRateProvider(_clientMock, _cacheMock, _clock);
            // Act
            var result = await sut.GetExchangeRates(currencies, default);

            // Assert
            var exchangeRates = result.ToList();
            Assert.Single(exchangeRates);
            Assert.Equivalent(exchangeRate, exchangeRates.First());
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsRatesFromAPI_WhenNotAvailableInCache()
        {
            // Arrange
            var currencies = new List<Currency> { new("USD") };
            var exchangeRate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 12);
            _cacheMock.GetCachedData<ExchangeRate>("USD/CZK").Returns(x => null, x => exchangeRate);
            _clientMock.GetRatesDaily(Arg.Any<DateOnly>(), Arg.Any<CancellationToken>()).Returns(new List<ExchangeRate> { exchangeRate });

            var sut = new ExchangeRateProvider(_clientMock, _cacheMock, _clock);
            // Act
            var result = await sut.GetExchangeRates(currencies, default);

            // Assert
            var exchangeRates = result.ToList();
            Assert.Single(exchangeRates);
            Assert.Equivalent(exchangeRate, exchangeRates.First());
        }

        [Fact]
        public async Task GetExchangeRates_SavesRatesToCache_WhenNotAvailableInCache()
        {
            // Arrange
            var currencies = new List<Currency> { new("USD") };
            var exchangeRate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 12);
            _cacheMock.GetCachedData<ExchangeRate>("USD/CZK").Returns(x => null, x => exchangeRate);
            _clientMock.GetRatesDaily(Arg.Any<DateOnly>(), Arg.Any<CancellationToken>()).Returns(new List<ExchangeRate> { exchangeRate });

            var sut = new ExchangeRateProvider(_clientMock, _cacheMock, _clock);
            // Act
            await sut.GetExchangeRates(currencies, default);

            // Assert
            _cacheMock.Received(1).SetCachedData("USD/CZK", exchangeRate);
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsEmptyList_WhenRequestNonExistentCurrency()
        {
            // Arrange
            var currencies = new List<Currency> { new("XYZ") };
            _cacheMock.GetCachedData<ExchangeRate>("XYZ/CZK").Returns(x => null, x => null);
            _clientMock.GetRatesDaily(Arg.Any<DateOnly>(), Arg.Any<CancellationToken>()).Returns(new List<ExchangeRate>());

            var sut = new ExchangeRateProvider(_clientMock, _cacheMock, _clock);

            // Act
            var result = await sut.GetExchangeRates(currencies, default);

            // Assert
            Assert.Empty(result);
        }
    }
}