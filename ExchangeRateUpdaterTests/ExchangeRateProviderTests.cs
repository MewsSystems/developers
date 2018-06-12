using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater;
using ExchangeRateUpdaterTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdaterTests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        private ExchangeRateProvider _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new ExchangeRateProvider(new List<IExchangeRatesSource>
            {
                new CnbExchangeRatesSource(new TestWebClientWrapper(), new Uri("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt"))
            });
        }

        [TestMethod]
        public void WhenGetExchangeRatesIsCalled_RequestedRatesAreReturned()
        {
            // setup
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("CZK"),
                new Currency("Rudolf"),
                new Currency("AUD")
            };

            // execute
            var result = _sut.GetExchangeRates(currencies).ToList();

            // assert
            Assert.AreEqual(2, result.Count);
            var usdczkRate = result.SingleOrDefault(x => x.TargetCurrency.Code == "USD" && x.SourceCurrency.Code == "CZK");
            Assert.IsNotNull(usdczkRate, "USD rate is missing in result");
            Assert.AreEqual(21.781m, usdczkRate.Value);
            var audczkRate = result.SingleOrDefault(x => x.TargetCurrency.Code == "AUD" && x.SourceCurrency.Code == "CZK");
            Assert.IsNotNull(audczkRate, "AUD rate is missing in result");
            Assert.AreEqual(16.566m, audczkRate.Value);
        }
    }
}
