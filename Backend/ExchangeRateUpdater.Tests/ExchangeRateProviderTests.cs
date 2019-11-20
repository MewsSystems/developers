using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        [TestMethod]
        public void Can_provide_requested_rates_When_available_from_source()
        {
            var aud = new Currency("AUD");
            var bgn = new Currency("BGN");
            var dkk = new Currency("DKK");
            var brl = new Currency("BRL");
            var czk = new Currency("CZK");

            var source = new ExchangeRateMemorySource(
                new ExchangeRate(aud, czk, 15.748M),
                new ExchangeRate(bgn, czk, 13.073M),
                new ExchangeRate(brl, czk, 5.503M));
            var provider = new ExchangeRateProvider(source);


            var result = provider.GetExchangeRates(new[] { aud, bgn, dkk }).GetAwaiter().GetResult();
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.SourceCurrency == aud), "AUD rate is in source data and is requested, must be in results");
            Assert.IsTrue(result.Any(x => x.SourceCurrency == bgn), "BGN rate is in source data and is requested, must be in results");
            Assert.IsFalse(result.Any(x => x.SourceCurrency == dkk), "DKK rate is not in source data and is requeste, cannot be in results");
            Assert.IsFalse(result.Any(x => x.SourceCurrency == brl), "BRL rate is in source data but was not requested, cannot be in results");
        }

        [TestMethod]
        public void Throws_When_source_returns_duplicate_rate()
        {
            var aud = new Currency("AUD");
            var bgn = new Currency("BGN");
            var czk = new Currency("CZK");

            var source = new ExchangeRateMemorySource(
                new ExchangeRate(aud, czk, 15.748M),
                new ExchangeRate(bgn, czk, 13.073M),
                new ExchangeRate(bgn, czk, 5.503M));
            var provider = new ExchangeRateProvider(source);

            Assert.ThrowsException<ExchangeRateProviderException>(() => provider.GetExchangeRates(new[] { aud, bgn }).GetAwaiter().GetResult());
        }
    }
}
