using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class CnbRateFeedParserTest
    {
        [TestMethod]
        public void CnbRatesFeedParser_StandardInputTest()
        {
            var unit = new CnbRateFeedParser();
            var input = @"11.02.2019 #29
země|měna|množství|kód|kurz
Austrálie|dolar|1|AUD|16,164
Brazílie|real|1|BRL|6,112";

            var result = unit.Parse(input).ToArray();

            
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("AUD", result[0].TargetCurrency.Code);
            Assert.AreEqual("CZK", result[0].SourceCurrency.Code);
            Assert.AreEqual(16.164M, result[0].Value);
            Assert.AreEqual(6.112M, result[1].Value);
        }
    }
}
