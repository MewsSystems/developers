using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Cnb;
using ExchangeRateUpdater.Model;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private static readonly IEnumerable<Currency> _currencies = new[]
{
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK")
        };

        private Mock<ICnbClient> _cnbClient;
        private Mock<IExchangeRateCache> _cache;

        private ExchangeRateProvider _exchangeRateProvider;

        [SetUp]
        public void Setup()
        {
            _cnbClient = new Mock<ICnbClient>();
            _cache = new Mock<IExchangeRateCache>();

            _exchangeRateProvider = new ExchangeRateProvider(_cnbClient.Object, _cache.Object);
        }

        [Test]
        public async Task GivenNoCacheWhenGettingExchangeRatesThenFetchesMapsAndFiltersRatesCorrectly()
        {
            // Arrange
            _cache
                .Setup(c => c.GetValue())
                .Returns<ExchangeRate[]?>(null);

            var cnbRates = new List<Cnb.Dtos.ExchangeRate>
            {
                    new Cnb.Dtos.ExchangeRate("US", "dollar", 1, "USD", "CZK", 12.3m),
                    new Cnb.Dtos.ExchangeRate("Iceland", "krona", 100, "ISK", "CZK", 17.434m),
                    new Cnb.Dtos.ExchangeRate("EU", "euro", 10, "EUR", "CZK", 4.5m),
            };

            var response = new Cnb.Dtos.DailyExchangeRates(DateOnly.FromDateTime(DateTime.Today), cnbRates);

            _cnbClient
                .Setup(c => c.GetLatestExchangeRatesAsync())
                .ReturnsAsync(response);

            // Act
            var rates = await _exchangeRateProvider.GetExchangeRates(_currencies);

            // Assert
            var expectedRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 12.3m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 0.45m),
            };

            rates.Should().BeEquivalentTo(expectedRates);

            _cnbClient.VerifyAll();
            _cache.VerifyAll();
        }

        [Test]
        public async Task GivenNoCacheWhenGettingExchangeRatesThenSetsFetchedRatesToCache()
        {
            // Arrange
            _cache
                .Setup(c => c.GetValue())
                .Returns<ExchangeRate[]?>(null);

            ExchangeRate[]? setRates = null;
            _cache
                .Setup(c => c.Set(It.IsAny<ExchangeRate[]>()))
                .Callback<ExchangeRate[]>(r => setRates = r);

            var cnbRates = new List<Cnb.Dtos.ExchangeRate>
            {
                    new Cnb.Dtos.ExchangeRate("US", "dollar", 1, "USD", "CZK", 12.3m),
                    new Cnb.Dtos.ExchangeRate("Iceland", "krona", 100, "ISK", "CZK", 17.434m),
                    new Cnb.Dtos.ExchangeRate("EU", "euro", 10, "EUR", "CZK", 4.5m),
            };

            var response = new Cnb.Dtos.DailyExchangeRates(DateOnly.FromDateTime(DateTime.Today), cnbRates);

            _cnbClient
                .Setup(c => c.GetLatestExchangeRatesAsync())
                .ReturnsAsync(response);

            // Act
            await _exchangeRateProvider.GetExchangeRates(_currencies);

            // Assert
            var expectedRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 12.3m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 0.45m),
                new ExchangeRate(new Currency("ISK"), new Currency("CZK"), 0.17434m),
            };

            setRates.Should().BeEquivalentTo(expectedRates);

            _cnbClient.VerifyAll();
            _cache.VerifyAll();
        }

        [Test]
        public async Task GivenCachedRatesWhenGettingExchangeRatesThenTakesCachedRatesAndFilters()
        {
            // Arrange
            var cachedRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 12.3m),
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 100m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 0.45m),
            };

            _cache
                .Setup(c => c.GetValue())
                .Returns(cachedRates);

            // Act
            var rates = await _exchangeRateProvider.GetExchangeRates(_currencies);

            // Assert
            var expectedRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 12.3m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 0.45m),
            };

            rates.Should().BeEquivalentTo(expectedRates);

            _cnbClient.VerifyNoOtherCalls();
            _cache.VerifyAll();
        }

        [Test]
        public void WhenGettingExchangeRatesFailsThenThrowsCustomException()
        {
            // Arrange
            _cache
                .Setup(c => c.GetValue())
                .Returns<ExchangeRate[]?>(null);

            var clientException = new Exception("Network error");
            _cnbClient
                .Setup(c => c.GetLatestExchangeRatesAsync())
                .ThrowsAsync(clientException);

            // Act
            var exception = Assert.ThrowsAsync<ExchangeRateProviderException>(
                () => _exchangeRateProvider.GetExchangeRates(_currencies));

            // Assert
            exception.InnerException.Should().BeSameAs(clientException);
        }
    }
}
