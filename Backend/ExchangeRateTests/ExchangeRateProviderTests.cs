using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateTests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        [TestMethod]
        public async Task ReturnsEmptyWhenPassedEmptyList()
        {
            var provider = new ExchangeRateProvider();
            var result = await provider.GetExchangeRates(Enumerable.Empty<Currency>());
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public async Task ReturnsEmptyWhenPassedSingleCurrency()
        {
            var currencies = new[] { new Currency("CZK") };
            var provider = new ExchangeRateProvider();
            var result = await provider.GetExchangeRates(currencies);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public async Task ReturnsEmptyWhenPassedInvalidPairs()
        {
            var currencies = new[] { new Currency("CZK"), new Currency("XEW"), new Currency("DDX"), new Currency("ABC") };
            var provider = new ExchangeRateProvider();
            var result = await provider.GetExchangeRates(currencies);
            Assert.AreEqual(result.Count(), 0);
        }

        [TestMethod]
        public async Task ReturnsRatesWhenPassedSinglePair()
        {
            var currencies = new[] { new Currency("CZK"), new Currency("EUR") };
            var provider = new ExchangeRateProvider();
            var result = await provider.GetExchangeRates(currencies);
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public async Task ReturnsMultipleRatesWhenPassedMultipleCurrencies()
        {
            var currencies = new[] { new Currency("CZK"), new Currency("USD"), new Currency("EUR"), new Currency("HUF") };
            var provider = new ExchangeRateProvider();
            var result = await provider.GetExchangeRates(currencies);
            Assert.IsTrue(result.Count() > 4);
        }
    }
}
