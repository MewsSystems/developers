using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Utils;
using FluentAssertions;
using FluentAssertions.Execution;

namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class ExchangeRateFilterTests
    {
        #region Test Data Fields
        private readonly ExchangeRate _usdCzkExchangeRate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.32m);

        private readonly ExchangeRate _eurCzkExchangeRate = new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.84m);

        private readonly ExchangeRate _gbpCzkExchangeRate = new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 29.79m);

        private readonly Currency _usd = new("USD");

        private readonly Currency _eur = new("EUR");

        private readonly Currency _xyz = new("XYZ");
        #endregion

        [TestMethod]
        public void FilterByCurrencies_WithMatchingCurrencies_ReturnMatchedCurrencies()
        {
            // Arrange
            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate> { _usdCzkExchangeRate, _eurCzkExchangeRate, _gbpCzkExchangeRate };
            IEnumerable<Currency> currencies = new List<Currency> { _usd, _eur, _xyz };

            // Act
            IEnumerable<ExchangeRate> resultExchangeRates = ExchangeRateFilter.FilterByCurrencies(exchangeRates, currencies);

            // Assert
            using (new AssertionScope())
            {
                resultExchangeRates.Should().HaveCount(2);
                resultExchangeRates.Should().Contain(_usdCzkExchangeRate);
                resultExchangeRates.Should().Contain(_eurCzkExchangeRate);
            }
        }

        [TestMethod]
        public void FilterByCurrencies_WithNoMatchingCurrencies_ReturnEmptyList()
        {
            // Arrange
            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate> { _eurCzkExchangeRate, _gbpCzkExchangeRate };
            IEnumerable<Currency> currencies = new List<Currency> { _usd, _xyz };

            // Act
            IEnumerable<ExchangeRate> resultExchangeRates = ExchangeRateFilter.FilterByCurrencies(exchangeRates, currencies);

            // Assert
            resultExchangeRates.Should().BeEmpty();
        }

        [TestMethod]
        public void FilterByCurrencies_WithEmptyExchangeRates_ReturnsEmptyList()
        {
            // Arrange
            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            IEnumerable<Currency> currencies = new List<Currency> { _usd, _xyz };

            // Act
            IEnumerable<ExchangeRate> resultExchangeRates = ExchangeRateFilter.FilterByCurrencies(exchangeRates, currencies);

            // Assert
            resultExchangeRates.Should().BeEmpty();
        }

        [TestMethod]
        public void FilterByCurrencies_WithEmptyCurrencies_ThrowsArgumentException()
        {
            // Arrange
            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate> { _eurCzkExchangeRate, _gbpCzkExchangeRate };
            IEnumerable<Currency> currencies = new List<Currency>();

            // Act
            Action act = () => ExchangeRateFilter.FilterByCurrencies(exchangeRates, currencies);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void FilterByCurrencies_WithNullCurrencies_ThrowsArgumentException()
        {
            // Arrange
            IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate> { _eurCzkExchangeRate, _gbpCzkExchangeRate };

            // Act
            Action act = () => ExchangeRateFilter.FilterByCurrencies(exchangeRates, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
