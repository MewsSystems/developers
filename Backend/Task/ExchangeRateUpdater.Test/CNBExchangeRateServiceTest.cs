using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExchangeRateService;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ExchangeRateUpdater.Test
{
    [TestClass]
    public class CNBExchangeRateServiceTest
    {
        [TestMethod]
        public void GetExchangeRateDataValid()
        {
            var cnbExchangeRateService = new CNBExchangeRateService();
            var currencyCodes = new List<string> { "EUR", "GBP", "JPY" };
            List<CurrencyData> actual = cnbExchangeRateService.GetExchangeRateData(currencyCodes).ToList();

            Assert.IsTrue(actual.Count == 3);
            foreach (var currencyData in actual)
            {
                Assert.IsTrue(currencyCodes.Contains(currencyData.CurrencyCode));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetExchangeRateDataNoDataAvailableThrowsApplicationException()
        {
            var cnbExchangeRateService = new CNBExchangeRateService("INVALID_URL");
            var currencyCodes = new List<string> { "GBP", "EUR", "JPY" };

            var actual = cnbExchangeRateService.GetExchangeRateData(currencyCodes);
        }
    }
}
