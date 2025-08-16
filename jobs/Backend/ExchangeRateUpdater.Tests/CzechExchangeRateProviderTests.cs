using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class CzechExchangeRateProviderTests
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        [TestMethod]
        public async Task GetCzechExchangeRateAsync_SuccessfulRequest_ReturnsRate()
        {
            var provider = new ExchangeRateProvider(HttpClient);

            var currencies = new List<Currency> { new Currency("CZK"), new Currency("USD") };
            var date = DateTime.Now.Date;
            var rates = await provider.GetCzechExchangeRateAsync(currencies, date);

            Assert.IsNotNull(rates);
            Assert.AreEqual(1, rates.Count);
        }

        [TestMethod]
        public async Task GetCzechExchangeRateAsync_SuccessfulRequest_ReturnsMultipleRate()
        {
            var provider = new ExchangeRateProvider(HttpClient);

            var currencies = new List<Currency> { new Currency("CZK"), new Currency("USD"), new Currency("EUR"), new Currency("JPY") };
            var date = DateTime.Now.Date;
            var rates = await provider.GetCzechExchangeRateAsync(currencies, date);

            Assert.IsNotNull(rates);
            Assert.AreEqual(3, rates.Count);
        }

        [TestMethod]
        public async Task GetCzechExchangeRateAsync_SuccessfulRequest_ReturnsEmptyList()
        {
            var provider = new ExchangeRateProvider(HttpClient);

            var currencies = new List<Currency> { new Currency("CZK"), new Currency("XYZ") };
            var date = DateTime.Now.Date;
            var rates = await provider.GetCzechExchangeRateAsync(currencies, date);

            Assert.IsNotNull(rates);
            Assert.AreEqual(0, rates.Count);
        }

        [TestMethod]
        public async Task GetCzechExchangeRateAsync_SuccessfulRequest_ReturnsCorrectRate()
        {
            var provider = new ExchangeRateProvider(HttpClient);

            var currencies = new List<Currency> { new Currency("CZK"), new Currency("USD") };
            var date = Convert.ToDateTime("2023-10-10");
            var rates = await provider.GetCzechExchangeRateAsync(currencies, date);

            Assert.IsNotNull(rates);
            Assert.AreEqual(1, rates.Count);
            Assert.AreEqual(Convert.ToDecimal(23.212), rates[0].Value);
        }
    }
}