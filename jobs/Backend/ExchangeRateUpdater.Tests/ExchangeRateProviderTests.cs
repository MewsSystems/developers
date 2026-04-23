using Moq;
using Moq.Protected;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        private Mock<HttpMessageHandler> _mockHandler;
        private ExchangeRateProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            // Mock HttpMessageHandler
            _mockHandler = new Mock<HttpMessageHandler>();
            _mockHandler.Protected()
                // Setup method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // Prepare expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(
                         "{\"rates\":[" +
                         "{\"validFor\":\"2024-01-02\",\"order\":1,\"country\":\"Japan\",\"currency\":\"yen\",\"amount\":100,\"currencyCode\":\"JPY\",\"rate\":15.862}," +
                         "{\"validFor\":\"2024-01-02\",\"order\":1,\"country\":\"USA\",\"currency\":\"dollar\",\"amount\":1,\"currencyCode\":\"USD\",\"rate\":22.526}," +
                         "{\"validFor\":\"2024-01-02\",\"order\":1,\"country\":\"Mexico\",\"currency\":\"peso\",\"amount\":1,\"currencyCode\":\"MXN\",\"rate\":1.322}" +
                         "]}")
                })
                .Verifiable();

            // Use mock HttpMessageHandler to create an HttpClient
            var httpClient = new HttpClient(_mockHandler.Object);
            _provider = new ExchangeRateProvider(httpClient);
        }

        [TestMethod]
        public async Task GetExchangeRates_OnlyValidCurrencies()
        {
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("JPY")
            };

            var rates = (await _provider.GetExchangeRates(currencies)).ToList();

            Assert.AreEqual(2, rates.Count, "Should return rates for 2 currencies.");
            Assert.IsTrue(rates.Any(rate => rate.TargetCurrency.Code == "USD" && rate.Value == 22.526m), "Should include rate for USD with expected value.");
            Assert.IsTrue(rates.Any(rate => rate.TargetCurrency.Code == "JPY" && rate.Value == 0.15862m), "Should include rate for JPY with expected value.");

            // Verify that the SendAsync method was called exactly once
            _mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(), // Ensures that the method was called exactly once
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get  // Ensures the HTTP method is GET
                    && req.RequestUri.ToString().Contains("https://api.cnb.cz/cnbapi/exrates/daily")), // You can customize this condition based on your API endpoint
                ItExpr.IsAny<CancellationToken>());

        }

        [TestMethod]
        public async Task GetExchangeRates_NoValidCurrencies()
        {

            var currencies = new List<Currency>
            {
                new Currency("BAD"),
                new Currency("EWW")
            };

            var rates = (await _provider.GetExchangeRates(currencies)).ToList();

            Assert.AreEqual(0, rates.Count, "Should return rates for 0 currencies.");

            _mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get  
                    && req.RequestUri.ToString().Contains("https://api.cnb.cz/cnbapi/exrates/daily")), // You can customize this condition based on your API endpoint
                ItExpr.IsAny<CancellationToken>());



        }

        [TestMethod]
        public async Task GetExchangeRates_BothValidAndInvalidCurrencies()
        {
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("BAD"),
                new Currency("MXN")
            };

            var rates = (await _provider.GetExchangeRates(currencies)).ToList();

            Assert.AreEqual(2, rates.Count, "Should return rates for 2 currencies.");
            Assert.IsTrue(rates.Any(rate => rate.TargetCurrency.Code == "USD" && rate.Value == 22.526m), "Should include rate for USD with expected value.");
            Assert.IsTrue(rates.Any(rate => rate.TargetCurrency.Code == "MXN" && rate.Value == 1.322m), "Should include rate for JPY with expected value.");

            // Verify that the SendAsync method was called exactly once
            _mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(), // Ensures that the method was called exactly once
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get  // Ensures the HTTP method is GET
                    && req.RequestUri.ToString().Contains("https://api.cnb.cz/cnbapi/exrates/daily")), // You can customize this condition based on your API endpoint
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task GetExchangeRates_NoCurrenciesProvided()
        {
            var currencies = new List<Currency>
            {
            };

            var rates = (await _provider.GetExchangeRates(currencies)).ToList();

            Assert.AreEqual(0, rates.Count, "Should return rates for 0 currencies.");

            _mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get
                    && req.RequestUri.ToString().Contains("https://api.cnb.cz/cnbapi/exrates/daily")), // You can customize this condition based on your API endpoint
                ItExpr.IsAny<CancellationToken>());
        }
    }
}