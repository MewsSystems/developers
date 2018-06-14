using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater;
using ExchangeRateUpdaterTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdaterTests
{
    [TestClass]
    
    public class ExchangeRateProviderIntegrationTests
    {
        private ExchangeRateProvider _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new ExchangeRateProvider(new List<IExchangeRatesSource>
            {
                new CnbExchangeRatesSource(new WebClientWrapper(), new Uri("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt"))
            });
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void WhenGetExchangeRatesIsCalled_ResultsAreReturnedAndExceptionIsNotThrown()
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
            Assert.IsTrue(result.Any());
        }
    }
}