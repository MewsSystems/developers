using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {

        public ExchangeRateProvider ExchangeRateProvider { get; set; }
        public static string SourceUrl { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            GetSourceUrl();
            ExchangeRateProvider = new ExchangeRateProvider();
        }
        public void GetSourceUrl()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();
            SourceUrl = config.GetSection("CentralBank").GetSection("SourceUrl").Value;
        }

        [Test]
        public void GetXmlAttribute_HandlesNullValue()
        {
            XDocument xDoc = new();
            xDoc.Add(new XElement("Test"));
            var xmlAttribute = ExchangeRateProvider.GetXmlAttribute("test", xDoc.Descendants().First());
            Assert.AreEqual(string.Empty, xmlAttribute);
        }
        
        [Test]
        public async Task GetXmlSource_ReturnsDocAsync()
        {
            Object document = await ExchangeRateProvider.GetXmlSource(SourceUrl);
            Assert.AreEqual(typeof(XDocument), document.GetType());
        }
        
        [Test]
        public void GetExchangeRates_ReturnsNotEmptyList()
        {
            var exchangeRates = ExchangeRateProvider.GetExchangeRates(new List<Currency>(), SourceUrl);
            Assert.AreNotEqual(typeof(List<ExchangeRate>), exchangeRates.GetType());
        }
    }
}

