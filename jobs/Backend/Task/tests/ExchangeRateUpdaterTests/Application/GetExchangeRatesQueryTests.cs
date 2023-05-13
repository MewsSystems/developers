using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdaterTests.Application
{
    public class GetExchangeRatesQueryTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_When_CurrenciesIsNull_Then_ThrowsArgumentException()
        {
            // Arrange            
            var query = new GetExchangeRatesQuery(new Mock<IExchangeRateProvider>().Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await query.ExecuteAsync(null!));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_When_CurrenciesIsNotNull_Then_CallsIExchangeRateProvider()
        {
            // Arrange            
            var currencies = new List<Currency> { new Currency("USD") };
            var rateProvider = new Mock<IExchangeRateProvider>();
            var query = new GetExchangeRatesQuery(rateProvider.Object);

            // Act
            await query.ExecuteAsync(currencies);

            // Assert
            rateProvider.Verify(rp => rp.GetExchangeRatesAsync(currencies), Times.Once);            
        }

        [Fact]
        public async Task GetExchangeRatesAsync_When_IExchangeRateProviderReturnsExchangeRates_Then_ReturnsExchangeRates()
        {
            // Arrange            
            var expectedExchangeRates = new List<ExchangeRate> { new ExchangeRate(new Currency(""), new Currency(""), (decimal)1.3) };
            var currencies = new List<Currency> { new Currency("USD") };
            var rateProvider = new Mock<IExchangeRateProvider>();
            rateProvider.Setup(rp => rp.GetExchangeRatesAsync(currencies)).ReturnsAsync(expectedExchangeRates);

            var query = new GetExchangeRatesQuery(rateProvider.Object);

            // Act
            IEnumerable<ExchangeRate> exchangeRates = await query.ExecuteAsync(currencies);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.Single(exchangeRates);
        }
    }
}
