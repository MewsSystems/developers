using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests.Models
{
    [TestFixture]
    public class ExchangeRateTests
    {
        [Test]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            decimal value = 1.5m;

            // Act
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            Assert.AreEqual(sourceCurrency, exchangeRate.SourceCurrency);
            Assert.AreEqual(targetCurrency, exchangeRate.TargetCurrency);
            Assert.AreEqual(value, exchangeRate.Value);
        }

        [Test]
        public void ToString_ReturnsExpectedString()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            decimal value = 1.5m;
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Act
            var result = exchangeRate.ToString();

            // Assert
            Assert.AreEqual("USD/EUR=1.5", result);
        }

        [Test]
        public void ExchangeRate_Constructor_SetsSourceCurrency()
        {
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 1.5m;

            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            Assert.AreEqual(sourceCurrency, exchangeRate.SourceCurrency);
        }

        [Test]
        public void ExchangeRate_Constructor_SetsTargetCurrency()
        {
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 1.5m;

            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            Assert.AreEqual(targetCurrency, exchangeRate.TargetCurrency);
        }

        [Test]
        public void ExchangeRate_Constructor_SetsValue()
        {
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 1.5m;

            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            Assert.AreEqual(value, exchangeRate.Value);
        }

        [Test]
        public void ExchangeRate_ToString_ReturnsExpectedFormat()
        {
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 1.5m;

            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            Assert.AreEqual("USD/EUR=1.5", exchangeRate.ToString());
        }

        [Test]
        public void ExchangeRate_Rate_SetsAndGetsCorrectly()
        {
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 1.5m;

            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            exchangeRate.Rate = 1.3m;

            Assert.AreEqual(1.3m, exchangeRate.Rate);
        }
    }

}