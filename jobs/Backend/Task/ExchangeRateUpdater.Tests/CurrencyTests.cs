using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class CurrencyTests
    {
        [Test]
        public void ToString_ReturnsCode()
        {
            var currency = new Currency("USD");
            Assert.That(currency.ToString(), Is.EqualTo("USD"));
        }
    }
}
