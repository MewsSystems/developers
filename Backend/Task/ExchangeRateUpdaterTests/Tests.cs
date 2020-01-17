using ExchangeRateUpdater;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests
{
    public class Tests
    {
        Mock<IHttpClientFactory> mockFactory;

        [SetUp]
        public void Setup()
        {
            mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(a => a.CreateClient(It.IsAny<string>())).Returns(new HttpClient(new MockHttpMessageHandler()));
        }

        [Test]
        public void GetRatesForTwoCurrencies()
        {
            IEnumerable<Currency> validCurrencies = new[]
            {
            new Currency("USD"),
            new Currency("EUR")
            };

            CNBExchangeRateProvider _provider = new CNBExchangeRateProvider(mockFactory.Object, validCurrencies);

            var rates = _provider.GetExchangeRates();
            int count = 0;
            foreach (var rate in rates)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        [Test]
        [TestCase("US1")]
        [TestCase("US")]
        [TestCase("USDD")]
        [TestCase("US?")]
        public void InvalidCurrencies(string code)
        {
            Assert.Throws<ArgumentException>(()=>new CNBExchangeRateProvider(mockFactory.Object, new[]{new Currency(code)}));
        }

        [Test]
        public void GetRatesWhileOneCurrencyDoesntExist()
        {
            IEnumerable<Currency> validCurrencies = new[]
            {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("XXX")
            };

            CNBExchangeRateProvider _provider = new CNBExchangeRateProvider(mockFactory.Object, validCurrencies);

            var rates = _provider.GetExchangeRates();
            int count = 0;
            foreach (var rate in rates)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        [Test]
        public void CurrenciesAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CNBExchangeRateProvider(mockFactory.Object, null));
        }

        [Test]
        public void HttpClientFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CNBExchangeRateProvider(null, new[] { new Currency("USD") }));
        }
    }


    public class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var expectedResponse = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "test.xml"));

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedResponse)
            };

            return await Task.FromResult(responseMessage);
        }
    }
}