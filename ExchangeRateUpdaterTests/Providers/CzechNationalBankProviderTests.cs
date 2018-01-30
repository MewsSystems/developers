using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestClass()]
    public class CzechNationalBankProviderTests
    {

        [TestMethod()]
        public void GetExchangeRatesTestEx()
        {
            //Arrange
            var specificProvider = new CzechNationalBankProvider();
            //Act
            Action getExchangeRates = () => specificProvider.GetExchangeRates(null).Count();
            //Assert
            Assert.ThrowsException<Exception>(getExchangeRates);
        }

        [TestMethod()]
        public void GetExchangeRatesTestOk()
        {
            //Arrange
            var specificProvider = new CzechNationalBankProvider();
            IEnumerable<Currency> currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
            };
            //Act
            int count= specificProvider.GetExchangeRates(currencies).Count();
            //Assert
            Assert.IsTrue(count == 2);
        }
    }
}