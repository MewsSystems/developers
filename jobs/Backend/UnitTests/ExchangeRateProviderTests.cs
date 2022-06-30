using static ExchangeRateUpdater.Program;

namespace ExchangeRateUpdater
{
    [TestClass]
    public class ExchangeRateProviderTest
    {
        [TestMethod]
        public void GetExchangeRates_ShouldReturnCertainResultCount()
        {
            // Arrange
            var currencyPairs = new[]
            {
                new CurrencyPair(new Currency("CZK"), new Currency("USD")),
                new CurrencyPair(new Currency("CZK"), new Currency("EUR")),
                new CurrencyPair(new Currency("CZK"), new Currency("CZK")),
                new CurrencyPair(new Currency("EUR"), new Currency("USD")),
                new CurrencyPair(new Currency("GBP"), new Currency("PLN"))
            };
            // Act
            var result = ExchangeRateProvider.GetExchangeRatesAsync(currencyPairs).Result;
            // Assert
            Assert.AreEqual(currencyPairs.Length, result.Count());
        }

        [TestMethod]
        public void GetExchangeRates_ShouldIgnoreNotExistingCodes()
        {
            // Arrange
            var currencyPairs = new[]
            {
                new CurrencyPair(new Currency("CZK"), new Currency("ERU")),
                new CurrencyPair(new Currency("DSU"), new Currency("CKZ"))
            };
            // Act
            var result = ExchangeRateProvider.GetExchangeRatesAsync(currencyPairs).Result;
            // Assert
            Assert.AreEqual(0, result.Count());
            
        }
    }
}