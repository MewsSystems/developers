using API.Models;

namespace API.UnitTests
{
    public class ExchangeRateTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var sourceCurrency = new Currency("CZK");
            var targetCurrency = new Currency("GBP");
            decimal value = 29.013m;

            // Act
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            Assert.Equal(sourceCurrency, exchangeRate.SourceCurrency);
            Assert.Equal(targetCurrency, exchangeRate.TargetCurrency);
            Assert.Equal(value, exchangeRate.Value);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var sourceCurrency = new Currency("CZK");
            var targetCurrency = new Currency("GBP");
            decimal value = 29.013m;
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);
            string expectedString = "CZK/GBP=29.013";

            // Act
            var result = exchangeRate.ToString();

            // Assert
            Assert.Equal(expectedString, result);
        }
    }
}
