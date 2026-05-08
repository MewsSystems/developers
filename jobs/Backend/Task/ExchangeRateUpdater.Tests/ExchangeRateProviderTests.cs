using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// Tests for ExchangeRateProvider - focused on orchestration rules only.
    /// </summary>
    public class ExchangeRateProviderTests
    {
        private const string ValidCnbContent = @"14 Jan 2026 #9
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|23.456
Eurozone|euro|1|EUR|25.123";

        [Fact]
        public void GetExchangeRates_WhenCzkNotRequested_ReturnsEmpty()
        {
            // Arrange: CZK is the target currency and must be in the request
            var provider = new ExchangeRateProvider(
                new FakeRatesSource(ValidCnbContent),
                NullLogger<ExchangeRateProvider>.Instance);
            var currencies = new[] { new Currency("USD"), new Currency("EUR") }; // No CZK

            // Act
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void GetExchangeRates_WhenCzkRequested_ReturnsMatchingRates()
        {
            // Arrange
            var provider = new ExchangeRateProvider(
                new FakeRatesSource(ValidCnbContent),
                NullLogger<ExchangeRateProvider>.Instance);
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };

            // Act
            var rates = provider.GetExchangeRates(currencies).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal("USD", rates[0].SourceCurrency.Code);
            Assert.Equal("CZK", rates[0].TargetCurrency.Code);
        }

        [Fact]
        public void GetExchangeRates_IsCaseInsensitiveForCurrencyCodes()
        {
            // Arrange: provider builds HashSet with OrdinalIgnoreCase
            var provider = new ExchangeRateProvider(
                new FakeRatesSource(ValidCnbContent),
                NullLogger<ExchangeRateProvider>.Instance);
            var currencies = new[] { new Currency("usd"), new Currency("czk") }; // lowercase

            // Act
            var rates = provider.GetExchangeRates(currencies).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal("USD", rates[0].SourceCurrency.Code);
        }

        [Fact]
        public void GetExchangeRates_ReturnsEmptyWhenSourceIsEmpty()
        {
            // Arrange
            var provider = new ExchangeRateProvider(
                new FakeRatesSource(""),
                NullLogger<ExchangeRateProvider>.Instance);
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };

            // Act
            var rates = provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(rates);
        }
    }
}
