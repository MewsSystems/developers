using ExchangeRateUpdater.Models;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.Models
{
    [TestFixture]
    public class CurrencyTests
    {
        [Test]
        public void Currency_Constructor_SetsCode()
        {
            // Arrange
            var code = "USD";

            // Act
            var currency = new Currency(code);

            // Assert
            Assert.AreEqual(code, currency.Code);
        }

        [Test]
        public void Currency_ToString_ReturnsCode()
        {
            // Arrange
            var code = "EUR";
            var currency = new Currency(code);

            // Act
            var result = currency.ToString();

            // Assert
            Assert.AreEqual(code, result);
        }

        [Test]
        public void Currency_CZK_ReturnsCZKCurrency()
        {
            // Arrange
            var expectedCode = "CZK";

            // Act
            var result = Currency.CZK;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCode, result.Code);
        }
    }
}
