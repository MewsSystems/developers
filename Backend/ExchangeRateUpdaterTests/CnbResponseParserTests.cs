using ExchangeRateUpdater.CNB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace ExchangeRateUpdaterTests
{
    [TestClass]
    public class CnbResponseParserTests
    {
        [TestMethod]
        public void ShouldParseExchangeRateTable()
        {
            // Arrange

            var data =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<kurzy banka=\"CNB\" datum=\"19.02.2021\" poradi=\"35\">" +
                    "<tabulka typ=\"XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU\">" +
                        "<radek kod=\"AUD\" mena=\"dolar\" mnozstvi=\"1\" kurz=\"16,733\" zeme=\"Austrálie\"/>" +
                        "<radek kod=\"BRL\" mena=\"real\" mnozstvi=\"1\" kurz=\"3,937\" zeme=\"Brazílie\"/>" +
                    "</tabulka>" +
                "</kurzy>";

            var parser = new CnbResponseParser();

            // Act

            ExchangeRatesCollectionDto result = null;
            using (var content = new MemoryStream(Encoding.UTF8.GetBytes(data ?? "")))
            {
                result = parser.Parse(content);
            }

            // Assert

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Table);
            Assert.AreEqual(2, result.Table.Length);

            Assert.AreEqual("AUD", result.Table[0].Code);
            Assert.AreEqual(1, result.Table[0].Quantity);
            Assert.AreEqual(16.733M, result.Table[0].Rate);

            Assert.AreEqual("BRL", result.Table[1].Code);
            Assert.AreEqual(1, result.Table[1].Quantity);
            Assert.AreEqual(3.937M, result.Table[1].Rate);
        }

        [TestMethod]
        public void ShouldReturnEmptyColelctionWhenThereAreNoRates()
        {
            // Arrange

            var data =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<kurzy banka=\"CNB\" datum=\"19.02.2021\" poradi=\"35\">" +
                    "<tabulka typ=\"XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU\">" +
                    "</tabulka>" +
                "</kurzy>";

            var parser = new CnbResponseParser();

            // Act

            ExchangeRatesCollectionDto result = null;
            using (var content = new MemoryStream(Encoding.UTF8.GetBytes(data ?? "")))
            {
                result = parser.Parse(content);
            }

            // Assert

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Table);
            Assert.AreEqual(0, result.Table.Length);
        }
    }
}
