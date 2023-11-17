using Mews.ExchangeRateProvider.Application.Repos;
using Mews.ExchangeRateProvider.Application.Utils;
using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;
using Mews.ExchangeRateProvider.Infrastructure.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRateProvider.Application.Test.Repos
{
    public class RateRepositoryTests
    {
        [Fact]
        public async Task GetDailyRatesAsync_WhenCacheExist_ShouldReturnCachedData()
        {
            // Arrange
            var cacheProviderMock = new Mock<ICNBCacheProvider>();
            var cnbClientMock = new Mock<ICNBClient>();
            var loggerMock = new Mock<ILogger<RateRepository>>();

            var repository = new RateRepository(cacheProviderMock.Object, loggerMock.Object, cnbClientMock.Object);

            var date = "2023-11-13";
            var lang = "en";
            var getAllRates = true;

            var cachedRates = new List<ExchangeRate> { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.0m) };

            cacheProviderMock.Setup(c => c.GetFromCache(It.IsAny<string>())).Returns(cachedRates);

            // Act
            var result = await repository.GetDailyRatesAsync(date, lang, getAllRates);

            // Assert
            Assert.Equal(cachedRates, result);
            cacheProviderMock.Verify(c => c.GetFromCache(It.IsAny<string>()), Times.Once);
            cnbClientMock.Verify(c => c.GetDailyRatesCNBAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            cacheProviderMock.Verify(c => c.SetCache(It.IsAny<string>(), It.IsAny<IEnumerable<ExchangeRate>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
        }

        [Fact]
        public async Task GetDailyRatesAsync_WhenCacheEmpty_ShouldCallCNBClient()
        {
            // Arrange
            var cacheProviderMock = new Mock<ICNBCacheProvider>();
            var cnbClientMock = new Mock<ICNBClient>();
            var loggerMock = new Mock<ILogger<RateRepository>>();

            var repository = new RateRepository(cacheProviderMock.Object, loggerMock.Object, cnbClientMock.Object);

            var date = "2023-11-13";
            var lang = "en";
            var getAllRates = true;

            cacheProviderMock.Setup(c => c.GetFromCache(It.IsAny<string>())).Returns((IEnumerable<ExchangeRate>)null);

            var cnbResponse = new List<ResponseExchangeRate>
                    {
                        new ResponseExchangeRate { CurrencyCode = "USD", Rate = 25.0m, Amount = 1 }
                    };

            cnbClientMock.Setup(c => c.GetDailyRatesCNBAsync(date, lang)).ReturnsAsync(cnbResponse);

            // Act
            var result = await repository.GetDailyRatesAsync(date, lang, getAllRates);

            // Assert
            var expectedExchangeRate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25.0m / 1);

            Assert.Single(result);
            Assert.Equal(expectedExchangeRate.SourceCurrency.Code, result.First().SourceCurrency.Code);
            Assert.Equal(expectedExchangeRate.TargetCurrency.Code, result.First().TargetCurrency.Code);
            Assert.Equal(expectedExchangeRate.Value, result.First().Value);

            cacheProviderMock.Verify(c => c.GetFromCache(It.IsAny<string>()), Times.Once);
            cnbClientMock.Verify(c => c.GetDailyRatesCNBAsync(date, lang), Times.Once);
            cacheProviderMock.Verify(c => c.SetCache(It.IsAny<string>(), It.IsAny<IEnumerable<ExchangeRate>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
        }

        [Theory]
        [InlineData("2023-11-13", "en", false, 9)] 
        public async Task GetDailyRatesAsync_WhenUsingValidCurrenciesList_ShouldReturnFilteredRates(string date, string lang, bool getAllRates, int expectedCount)
        {
            // Arrange
            var cacheProviderMock = new Mock<ICNBCacheProvider>();
            var loggerMock = new Mock<ILogger<RateRepository>>();
            var cnbClientMock = new Mock<ICNBClient>();

            var rateRepository = new RateRepository(cacheProviderMock.Object, loggerMock.Object, cnbClientMock.Object);

            cnbClientMock.Setup(client => client.GetDailyRatesCNBAsync(date, lang)).ReturnsAsync(ValidCurrenciesList.currencies.Select(currency => new ResponseExchangeRate { CurrencyCode = currency.Code, Rate = 1.0m, Amount = 1 }));

            // Act
            var result = await rateRepository.GetDailyRatesAsync(date, lang, getAllRates);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expectedCount, result.Count());
            Assert.All(result, rate =>
            {
                Assert.NotNull(rate.SourceCurrency);
                Assert.Contains(rate.SourceCurrency.Code, ValidCurrenciesList.currencies.Select(c => c.Code));
            });

            cnbClientMock.Verify(client => client.GetDailyRatesCNBAsync(date, lang), Times.Once);
            cacheProviderMock.Verify(cache => cache.GetFromCache(It.IsAny<string>()), Times.Once);
            cacheProviderMock.Verify(cache => cache.SetCache(It.IsAny<string>(), It.IsAny<IEnumerable<ExchangeRate>>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
        }     
    }
}