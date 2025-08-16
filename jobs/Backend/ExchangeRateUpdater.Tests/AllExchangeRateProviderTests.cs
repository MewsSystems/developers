using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class AllExchangeRateProviderTests
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [TestMethod]
        public async Task GetAllExchangeRatesAsync_ReturnsExchangeRates()
        {
            var provider = new ExchangeRateProvider(HttpClient);
            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var date = DateTime.Now.Date;

            var rates = await provider.GetExchangeRatesAsync(currencies, date);

            Assert.IsNotNull(rates);
            Assert.AreEqual(2, rates.Count);
        }

        [TestMethod]
        public async Task GetAllExchangeRatesAsync_NoMatchingCurrencies_ReturnsEmptyList()
        {
            var provider = new ExchangeRateProvider(HttpClient);
            var currencies = new List<Currency> { new Currency("EUR"), new Currency("XYZ") };
            var date = DateTime.Now.Date;

            var rates = await provider.GetExchangeRatesAsync(currencies, date);

            Assert.IsNotNull(rates);
            Assert.AreEqual(0, rates.Count);
        }

        [TestMethod]
        public async Task GetAllExchangeRateAsync_ReturnsExchangeRate()
        {
            var provider = new ExchangeRateProvider(HttpClient);
            var currencies = new List<Currency> { new Currency("CZK"), new Currency("EUR") };
            var date = Convert.ToDateTime("2023-10-10");

            var rates = await provider.GetExchangeRatesAsync(currencies, date);
            Assert.IsNotNull(rates);
            Assert.AreEqual(2, rates.Count);

            var rate1 = rates.Where(rate => rate.SourceCurrency.Code == "EUR");
            Assert.AreEqual(Convert.ToDecimal(24.45438905), rate1.ElementAt(0).Value);
        }
    }
}
