using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdaterTests
{
    public abstract class Tests
    {
        public abstract ExchangeRateProvider ExchangeRateProvider();
        public string sourceUrl { get; set; }
        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();
            sourceUrl = config.GetSection("CentralBank").GetSection("SourceUrl").Value;
        }

        [Test]
        public void GetXmlAttribute_HandlesNullValue()
        {
            XDocument xDoc = new XDocument();
            var xmlAttribute = ExchangeRateProvider().GetXmlAttribute("test", xDoc.Descendants("kurz").First());
            Assert.AreEqual(null, xmlAttribute);
        }
        
        [Test]
        public void GetXmlSource_ReturnsDoc()
        {
            
            Object document = ExchangeRateProvider().GetXmlSource(sourceUrl);
            Assert.AreEqual(document.GetType(), typeof(XDocument));
        }
        
        [Test]
        public void GetExchangeRates_NotReturnsEmptyList()
        {
            var exchangeRates = ExchangeRateProvider().GetExchangeRates(new List<Currency>(), sourceUrl);
            Assert.AreNotEqual(exchangeRates, new List<ExchangeRate>());
        }
    }
}

