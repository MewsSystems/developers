using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeRateApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateTests
{
    [TestClass]
    public class ExchangeRatesApiTests
    {
        [TestMethod]
        public async Task ReturnsValidResponseWhenPassedValidPair()
        {
            var client = new ExchangeRatesApiClient();
            var data = await client.GetExchangeRate(new Currency("USD"), new Currency("EUR"));
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public async Task ReturnsNullWhenPassedInvalidBaseCurrency()
        {
            var client = new ExchangeRatesApiClient();
            var data = await client.GetExchangeRate(new Currency("USD"), new Currency("XYZ"));
            Assert.IsNull(data);
        }

        [TestMethod]
        public async Task ReturnsNullWhenPassedInvalidTargetCurrency()
        {
            var client = new ExchangeRatesApiClient();
            var data = await client.GetExchangeRate(new Currency("XYZ"), new Currency("USD"));
            Assert.IsNull(data);
        }
    }
}