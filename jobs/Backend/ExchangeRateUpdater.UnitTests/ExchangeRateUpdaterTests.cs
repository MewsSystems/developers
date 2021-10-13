using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Data.Models;
using ExchangeRateUpdater.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class ExchangeRateUpdaterTests
    {


        [TestMethod]
        public void CheckIfObjectHasCorrectAttributesForDeserializingXML()
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(CNBConnection.GetXml(CNBConnection.ExchangeRateXml).Result);

            string code = typeof(Rate).GetProperty(nameof(Rate.Code)).GetCustomAttribute<XmlAttributeAttribute>(true).AttributeName;
            string amount = typeof(Rate).GetProperty(nameof(Rate.Amount)).GetCustomAttribute<XmlAttributeAttribute>(true).AttributeName;
            string eRate = typeof(Rate).GetProperty(nameof(Rate.ExchangeRate)).GetCustomAttribute<XmlAttributeAttribute>(true).AttributeName;

            XmlNode firstEl = doc.SelectSingleNode("/kurzy/tabulka/radek[1]");

            Assert.AreEqual(code, firstEl.Attributes["kod"].Name);
            Assert.AreEqual(amount, firstEl.Attributes["mnozstvi"].Name);
            Assert.AreEqual(eRate, firstEl.Attributes["kurz"].Name);
        }

        [TestMethod]
        public void DeserializeXMLFileToObjectTest_ReturnObjectIsObject()
        {
            var s = CNBConnection.GetXml(CNBConnection.ExchangeRateXml).Result;

            var returnObject = XmlDeserializer.DeserializeXMLFileToObject<CNBExchangeRate>(s);
            var list = returnObject.RatesTable.ExchangeRates;

            Assert.AreEqual("AUD", list[0].Code);
        }


        [TestMethod]
        public void GetOnlyValidExchangeRatesTest()
        {

            IEnumerable<Currency> currencies = new[]
         {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
            var returnObject = XmlDeserializer.DeserializeXMLFileToObject<CNBExchangeRate>(CNBConnection.GetXml(CNBConnection.ExchangeRateXml).Result);
            var list = returnObject.RatesTable.ExchangeRates;

            var matchedListCount = currencies.Where(currency => list.Select(rate => rate.Code).Contains(currency.Code)).Count();
            var sourceCurrencyCount = currencies.Count();

            Assert.IsTrue(sourceCurrencyCount >= matchedListCount);

        }
    }
}
