using ExchangeRateUpdater.Exchange_Providers.Models;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRate_Tests
    {
        [Fact]
        public void ExchangeRate_ToString_ReturnsExpectedString()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            decimal value = 1.2m;
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Act
            string result = exchangeRate.ToString();

            // Assert
            Assert.Equal("USD/EUR=1,2", result);
        }
    }
}
