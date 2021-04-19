using ExchangeRateUpdater;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {
        [SetUp]
        public void Setup()
        {
         
        }

        [Test]
        public void ShouldReturnSuccesfulRatesOnlyFromResponse()
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

            string xml ="<kurzy banka=\"CNB\" datum=\"14.04.2021\" poradi=\"72\">\n" +
                            "<tabulka typ=\"XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU\">\n" +
                                "<radek kod=\"EUR\" mena=\"euro\" mnozstvi=\"1\" kurz=\"25,940\" zeme=\"EMU\"/>\n" +
                                "<radek kod=\"USD\" mena=\"dolar\" mnozstvi=\"1\" kurz=\"21,669\" zeme=\"USA\"/>\n" +
                            "</tabulka>\n" +
                        "</kurzy>\n";

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            httpResponse.StatusCode = System.Net.HttpStatusCode.OK;
            httpResponse.Content = new StringContent(xml);

            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            mockHttpClientWrapper.Setup(t => t.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(httpResponse);

            var exchangeProvider = new ExchangeRateProvider(mockHttpClientWrapper.Object);
            var results = exchangeProvider.GetExchangeRates(currencies);

            Assert.AreEqual(results.Count(), 9);
            Assert.AreEqual(results.Where(c => c.Success).Count(), 2);
            Assert.AreEqual(results.Where(c => !c.Success).Count(), 7);
            Assert.AreEqual(results.Where(c => c.Rate.TargetCurrency.Code == "USD").FirstOrDefault().Rate.Value, 21.669);
        }
    }
}