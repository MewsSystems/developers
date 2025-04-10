using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Tests.UnitTests
{
    public class CNBRateProviderTests
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly Mock<ICacheService> _mockCache;
        private readonly Mock<ICustomHttpClient> _mockHttpClient;
        private readonly Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private readonly Mock<IOptions<ExchangeRateUpdaterConfiguration>> _mockConfiguration;

        public CNBRateProviderTests()
        {
            _mockHttpClient = new Mock<ICustomHttpClient>();
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockCache = new Mock<ICacheService>();
            _mockConfiguration = new Mock<IOptions<ExchangeRateUpdaterConfiguration>>();

            var emptyRates = new CNBRates();

            _mockHttpClient.Setup(x => x.GetAsync<CNBRates>(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(emptyRates);

            _mockCache.Setup(x => x.Set(It.IsAny<object>(), It.IsAny<CNBRates>(), It.IsAny<MemoryCacheEntryOptions>()));
            _mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out emptyRates)).Returns(false);
            _mockConfiguration.Setup(x => x.Value).Returns(new ExchangeRateUpdaterConfiguration());

            _exchangeRateProvider = new ExchangeRateProvider(_mockConfiguration.Object, _mockLogger.Object, _mockHttpClient.Object, _mockCache.Object);
        }

        [Fact]
        public async Task GetExchangeRates_CurrenciesNull_Throws()
        {
            // Arrange
            List<Currency> currencies = null;
            var date = DateTime.Now;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _exchangeRateProvider.GetExchangeRates(currencies, date, CancellationToken.None));
        }

        [Fact]
        public async Task GetExchangeRates_CurrenciesEmpty_ReturnsEmpty()
        {
            // Arrange
            var currencies = new List<Currency>();
            var date = DateTime.Now;
            // Act
            var rates  = await _exchangeRateProvider.GetExchangeRates(currencies, date, CancellationToken.None);

            // Assert
            Assert.Empty(rates);
        }

        [Fact]
        public async Task GetExchangeRates_CurrenciesNotExist_ReturnsEmpty()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("NOTEXIST") };
            var date = DateTime.Now;

            // Act
            var rates = await _exchangeRateProvider.GetExchangeRates(currencies, date, CancellationToken.None);

            // Assert
            Assert.Empty(rates);
        }

        [Fact]
        public async Task GetExchangeRates_CurrenciesExist_ReturnsValues()
        {

            // Arrange
            var currencyCode = "USD";
            var currencies = new List<Currency> { new Currency(currencyCode) };
            var date = DateTime.Now;
            var result = new CNBRates() 
            { 
                Rates = new List<CNBRate>() 
                { 
                    new CNBRate 
                    {
                        Amount = 1,
                        CurrencyCode = currencyCode,
                        Rate = 1.0m,                        
                    } 
                } 
            };
            _mockHttpClient.Setup(x => x.GetAsync<CNBRates>(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => result);
            var exchangeRateProvider = new ExchangeRateProvider(_mockConfiguration.Object, _mockLogger.Object, _mockHttpClient.Object, _mockCache.Object);

            // Act
            var rates = await exchangeRateProvider.GetExchangeRates(currencies, date, CancellationToken.None);

            // Assert
            Assert.Contains(rates, x => x.TargetCurrency.Code == currencyCode);
        }
    }
}
