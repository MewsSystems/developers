namespace ExchangeRateUpdaterTests
{
    using System.Net;
    using Moq;
    using CurrencyExchangeService.Interfaces;
    using ExchangeRateService;
    using Logger;

    [TestFixture]
    public class CNBExchangeRateProviderTests
    {
        // TODO: Add more tests for other methods
        [SetUp]
        public void Setup() { }

        [Test]
        public void GetExchangeRatesSuccess()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(
                new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent("Some result") }));
            IExchangeRateProvider<string> exchangeRateProvider = new CNBExchangeRateProvider(new FileLogger(), mockClient.Object);

            var task = exchangeRateProvider.GetExchangeRates();

            Assert.True(task.Result.Length > 0);
        }

        [Test]
        public void GetExchangeRatesInternalServerError()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(
                new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("Some result") }));
            IExchangeRateProvider<string> exchangeRateProvider = new CNBExchangeRateProvider(new FileLogger(), mockClient.Object);

            var task = exchangeRateProvider.GetExchangeRates();

            Assert.True(task.Result.Length == 0);
        }

        [Test]
        public void GetExchangeRatesNotFound()
        {
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(
                new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound, Content = new StringContent("Some result") }));
            IExchangeRateProvider<string> exchangeRateProvider = new CNBExchangeRateProvider(new FileLogger(), mockClient.Object);

            var task = exchangeRateProvider.GetExchangeRates();

            Assert.True(task.Result.Length == 0);
        }
    }
}