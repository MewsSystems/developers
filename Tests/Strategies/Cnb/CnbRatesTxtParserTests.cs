using ExchangeRateUpdater.ExchangeRateStrategies.Cnb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tests.Strategies.Cnb
{
    [TestClass]
    public class CnbRatesTxtParserTests
    {
        [TestMethod]
        public void ShouldParseContents()
        {
            var contents = "16.05.2018 #93\n" +
                           "země|měna|množství|kód|kurz\n" +
                           "Austrálie|dolar|1|AUD|16,229\n" +
                           "Japonsko|jen|100|JPY|19,677";

            var parser = new CnbRatesTxtParser();

            var rates = parser.Parse(contents).ToList();

            Assert.AreEqual(2, rates.Count);

            Assert.AreEqual("AUD", rates[0].CurrencyCode);
            Assert.AreEqual(1, rates[0].Amount);
            Assert.AreEqual(16.229m, rates[0].Rate);

            Assert.AreEqual("JPY", rates[1].CurrencyCode);
            Assert.AreEqual(100, rates[1].Amount);
            Assert.AreEqual(19.677m, rates[1].Rate);
        }
    }
}
