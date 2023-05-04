using NUnit.Framework;

namespace ExchangeRateUpdater.Test
{
    internal class CurrencyTest
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("AB")]
        [TestCase("WXYZ")]
        public void TestConstuctorWithInvalidCode(string invalidCurrencyCode) {
            var ex = Assert.Throws<ArgumentException>(() => new Currency(invalidCurrencyCode));
            Assert.AreEqual($"Invalid currency code '{invalidCurrencyCode}'. Expected a three-letter code.", ex.Message);
        }
        
        [TestCase("CZK")]
        [TestCase("USD")]
        public void TestConstuctorWithValidCurrencyCode(string currencyCode) {
            var testObj = new Currency(currencyCode);

            Assert.AreEqual(currencyCode, testObj.Code);
        }
    }
}
