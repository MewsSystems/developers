using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Cached;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    public class CachedCnbExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRates_ReturnsCorrectRatesAndUsesCache()
        {
            // Arrange
            var currencies = new[] { new Currency("USD"), new Currency("EUR") };
            var expectedRates = new[]
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 24.000m),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 26.000m)
            };

            var providerMock = new Mock<IExchangeRateProvider>();
            providerMock.Setup(x => x.GetExchangeRates(currencies, It.IsAny<CancellationToken>())).ReturnsAsync(expectedRates);

            var clockMock = new Mock<IClock>();
            clockMock.SetupGet(x => x.Today).Returns(DateOnly.Parse("2023-04-12"));

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var cachedProvider = new CachedCnbExchangeRateProvider(providerMock.Object, clockMock.Object, memoryCache);

            // Act
            var rates1 = await cachedProvider.GetExchangeRates(currencies);
            var rates2 = await cachedProvider.GetExchangeRates(currencies);

            // Assert
            Assert.True(expectedRates.SequenceEqual(rates1));
            Assert.True(expectedRates.SequenceEqual(rates2));
            providerMock.Verify(x => x.GetExchangeRates(currencies, It.IsAny<CancellationToken>()), Times.Once); // Ensure caching is used
        }
    }
}
