using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ExchangeRateUpdater.Parsing.Tests
{
    [TestClass]
    public class CNBExchangeRateParserTests
    {
        [TestMethod]
        [DataRow(null, 0)]
        [DataRow("", 0)]
        [DataRow("15.09.2021\n", 0)]
        [DataRow("15.09.2021\nzemě|měna|množství|kód|kurz", 0)]
        [DataRow("15.09.2021\nzemě|měna|množství|kód|kurz\n", 0)]
        [DataRow("15.09.2021\nzemě|měna|množství|kód|kurz\n\nBulharsko|lev|1|BGN|12,945", 1)]
        [DataRow("15.09.2021\nzemě|měna|množství|kód|kurz\n\nBulharsko|lev|1|BGN|12,945\n\nSingapur|dolar|1|SGD|15,964", 2)]
        public void ParseTest(string data, int expectedCount)
        {
            var parser = new CNBExchangeRateParser();
            var actualResult = parser.Parse(data);

            Assert.AreEqual(expectedCount, actualResult.Count());
        }
    }
}