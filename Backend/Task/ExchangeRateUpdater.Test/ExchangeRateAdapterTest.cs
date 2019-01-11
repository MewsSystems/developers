using System;
using System.Collections.Generic;
using ExchangeRateService;
using ExchangeRateAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace ExchangeRateUpdater.Test
{
    [TestClass]
    public class ExchangeRateAdapterTest
    {
        [TestMethod]
        public void InitialisationCreatesIExchangeRateService()
        {
            var rateAdapter = new RateAdapter();
            IExchangeRateService iExchangeRateService = rateAdapter.ExchangeRateService as IExchangeRateService;
            Assert.IsNotNull(iExchangeRateService);
        }

        [TestMethod]
        public void GetExchangeRateDataValid()
        {
            var rateAdapter = new RateAdapter();
            var currencies = new List<string> { "EUR", "GBP" };
            var exchangeRateServiceMock = new Mock<IExchangeRateService>();
            exchangeRateServiceMock.Setup(x => x.GetExchangeRateData(currencies)).Returns(new List<CurrencyData>
            {
                new CurrencyData("EUR", 25, 1),
                new CurrencyData("GBP", 30, 1),
                new CurrencyData("JPY", 30m, 10)
            });

            var expected = new List<ExchangeRate>
            {
                new ExchangeRate(
                    new Currency("EUR"), 
                    new Currency("CZK"), 25m),
                new ExchangeRate(
                    new Currency("GBP"), 
                    new Currency("CZK"), 30m),
                new ExchangeRate(
                    new Currency("JPY"),
                    new Currency("CZK"), 3m)
            };

            rateAdapter.ExchangeRateService = exchangeRateServiceMock.Object;

            var actual = rateAdapter.GetExchangeRateData(currencies, "CZK").ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for(int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected.ElementAt(i).SourceCurrency.Code, actual.ElementAt(i).SourceCurrency.Code);
                Assert.AreEqual(expected.ElementAt(i).TargetCurrency.Code, actual.ElementAt(i).TargetCurrency.Code);
                Assert.AreEqual(expected.ElementAt(i).Value, actual.ElementAt(i).Value);
            }            
        }

        [TestMethod]
        public void GetCalculatedExchangeRateDataValid()
        {
            var rateAdapter = new RateAdapter();
            var currencies = new List<string> { "EUR", "GBP" };
            var exchangeRateServiceMock = new Mock<IExchangeRateService>();
            exchangeRateServiceMock.Setup(x => x.GetExchangeRateData(currencies)).Returns(new List<CurrencyData>
            {
                new CurrencyData("EUR", 25, 1),
                new CurrencyData("GBP", 30, 1),
                new CurrencyData("CZK", 1, 1)
            });

            var expected = new List<ExchangeRate>
            {
                new ExchangeRate(
                    new Currency("CZK"),
                    new Currency("EUR"), 1m/25m),
                new ExchangeRate(
                    new Currency("CZK"),
                    new Currency("GBP"), 1m/30m),
                new ExchangeRate(
                    new Currency("EUR"),
                    new Currency("GBP"), 25m/30m),
                new ExchangeRate(
                    new Currency("EUR"),
                    new Currency("CZK"), 25m),
                new ExchangeRate(
                    new Currency("GBP"),
                    new Currency("EUR"), 30m/25m),
                new ExchangeRate(
                    new Currency("GBP"),
                    new Currency("CZK"), 30m)
            };

            rateAdapter.ExchangeRateService = exchangeRateServiceMock.Object;

            var actual = rateAdapter.GetExchangeRateData(currencies, "CZK", true).ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected.ElementAt(i).SourceCurrency.Code, actual.ElementAt(i).SourceCurrency.Code);
                Assert.AreEqual(expected.ElementAt(i).TargetCurrency.Code, actual.ElementAt(i).TargetCurrency.Code);
                Assert.AreEqual(expected.ElementAt(i).Value, actual.ElementAt(i).Value);
            }

        }
    }
}
