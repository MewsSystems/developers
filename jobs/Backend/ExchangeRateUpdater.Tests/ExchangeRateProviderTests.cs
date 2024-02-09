using System.Net;
using System.Net.Http;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private Mock<HttpMessageHandler> _mock;
        private List<Currency> _currencies;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<HttpMessageHandler>();
            _currencies = new List<Currency> { new Currency("BRL") };
        }

        [Test]
        public async Task GetExchangeRates()
        {
            _mock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""rates"": [
                        {""currencyCode"": ""BRL"", ""rate"": 5.796, ""amount"": 1},
                        {""currencyCode"": ""USD"", ""rate"": 23.234, ""amount"": 1},
                        {""currencyCode"": ""EUR"", ""rate"": 24.987, ""amount"": 1}
                    ]
                }")
            });

            var client = new HttpClient(_mock.Object);
            var provider = new ExchangeRateProvider(client);

            var data = await provider.GetExchangeRates(_currencies);

            Assert.That(data, Has.Exactly(1).Items);
            Assert.That(data, Has.Some.Matches<ExchangeRate>(x => x.SourceCurrency.Code == "BRL" && x.Value == 5.796m));
            Assert.Pass();
        }
    }
}