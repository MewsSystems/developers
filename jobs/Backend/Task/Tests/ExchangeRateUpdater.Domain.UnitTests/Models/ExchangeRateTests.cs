using System.Globalization;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Enums;
using Xunit;

namespace ExchangeRateUpdater.Domain.UnitTests.Models
{
    public class ExchangeRateTests
    {
        [Theory]
        [InlineData(CurrencyCode.USD, CurrencyCode.CZK, 50.35, "USD/CZK=50.35")]
        [InlineData(CurrencyCode.EUR, CurrencyCode.CZK, 3.22, "EUR/CZK=3.22")]
        [InlineData(CurrencyCode.CZK, CurrencyCode.USD, 0.84, "CZK/USD=0.84")]
        [InlineData(CurrencyCode.NOK, CurrencyCode.EUR, 5, "NOK/EUR=5")]
        public void ToString_ShouldReturnCorrectValue(CurrencyCode sourceCurrencyCode, CurrencyCode targetCurrencyCode, decimal value, string expectedResult)
        {
            // Arrange 
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            var subjectUnderTest = new ExchangeRate(new Currency(sourceCurrencyCode), new Currency(targetCurrencyCode), value);

            // Act
            var result = subjectUnderTest.ToString();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
