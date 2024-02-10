using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateTests
    {
        [Test]
        public void ToString_ReturnsSourceCurrencyTargetCurrencyValue()
        {
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 1.5m);
            Assert.That(exchangeRate.ToString(), Is.EqualTo("USD/EUR=1.5"));
        }
    }
}
