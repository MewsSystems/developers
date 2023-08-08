using FluentAssertions;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IExchangeRateCache> _cache;

        public ExchangeRateProviderTests()
        {
            _cache = new Mock<IExchangeRateCache>();
            _cache.Setup(x => x.GetCachedValuesAsync()).ReturnsAsync(new List<ExchangeRateRecord> {
                new ExchangeRateRecord("2023-08-07", 150, "India", "rupee", 1, "IDR", 1.454M),
                new ExchangeRateRecord("2023-08-07", 150, "USA", "dollar", 1, "USD", 22.074M),
                new ExchangeRateRecord("2023-08-07", 150, "United Kingdom", "pound", 1, "GBP", 28.141M)
            });
        }

        [Fact]
        public async void GetExchangeRates_NonExistingCurrency_ReturnNoValue()
        {
            // Arrange
            var sut = new ExchangeRateProvider(_cache.Object);
            var currencies = new List<Currency>
            {
                new Currency("YYY")
            };

            // Act
            var rates = await sut.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().BeEmpty();
        }

        [Fact]
        public async void GetExchangeRates_ExistingCurrency_ReturnOneRateForIndia()
        {
            // Arrange
            var sut = new ExchangeRateProvider(_cache.Object);
            var currencies = new List<Currency>
            {
                new Currency("IDR")
            };

            // Act
            var rates = await sut.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().HaveCount(1);
            rates.First().SourceCurrency.Code.Should().Be("IDR");
            rates.First().Value.Should().Be((await _cache.Object.GetCachedValuesAsync()).First().rate);
        }

        [Fact]
        public async void GetExchangeRates_AllExistingCurrency_ReturnAllThree()
        {
            // Arrange
            var sut = new ExchangeRateProvider(_cache.Object);
            var currencies = new List<Currency>
            {
                new Currency("IDR"),
                new Currency("USD"),
                new Currency("GBP")
            };

            // Act
            var rates = await sut.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().HaveCount(3);
        }

        [Fact]
        public async void GetExchangeRates_OneExistingOneNonExistingCurrencies_ReturnExistingOnly()
        {
            // Arrange
            var sut = new ExchangeRateProvider(_cache.Object);
            var currencies = new List<Currency>
            {
                new Currency("YYY"),
                new Currency("IDR")
            };

            // Act
            var rates = await sut.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().HaveCount(1);
            rates.First().SourceCurrency.Code.Should().Be("IDR");
            rates.First().Value.Should().Be((await _cache.Object.GetCachedValuesAsync()).First().rate);
        }

        [Fact]
        public async void GetExchangeRates_EmptyCurrencyList_ReturnNoValue()
        {
            // Arrange
            var sut = new ExchangeRateProvider(_cache.Object);
            var currencies = new List<Currency>();

            // Act
            var rates = await sut.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().BeEmpty();
        }

    }

}
