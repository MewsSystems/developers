using System.Net;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.HttpUtils;

namespace ExchnageRateProvider.Tests
{
    public class CnbClientTest
    {
        private TestHttpMessageHandler _httpMessageHandler;
        private CnbClient _cnbClient;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandler = new TestHttpMessageHandler();
            var httpClient = new HttpClient(_httpMessageHandler) { BaseAddress = new Uri("https://api.cnb.cz") };
            _cnbClient = new CnbClient(httpClient);

        }

        [Test]
        public void GetCurrentExchangeRates_RequestFailed_ThrowsExchangeRatesException()
        {
            _httpMessageHandler.SendAsyncCallback = request => Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            var ex = Assert.ThrowsAsync<ExchangeRatesException>(async () => await _cnbClient.GetCurrentExchangeRatesAsync(null));
            Assert.That(ex.Retriable, Is.EqualTo(true));
        }

        [Test]
        public void GetCurrentExchangeRates_ResponseFormatChanged_ThrowsExchangeRatesException()
        {
            _httpMessageHandler.SendAsyncCallback = request => Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""newRates"":[]}")
            }); ;
            var ex = Assert.ThrowsAsync<ExchangeRatesException>(async () => await _cnbClient.GetCurrentExchangeRatesAsync(new List<Currency>()));
            Assert.That(ex.Retriable, Is.EqualTo(false));
            Assert.That(ex.Message, Is.EqualTo("Unrecognized content of the response"));
        }

        [Test]
        public async Task GetCurrentExchangeRates_ResponseAsExpected_ThrowsExchangeRatesException()
        {
            _httpMessageHandler.SendAsyncCallback = request => Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""rates"":[{
                    ""validFor"": ""2023-07-01"",
                    ""order"": 124,
                    ""country"": ""Austrálie"",
                    ""currency"": ""dolar"",
                    ""amount"": 1,
                    ""currencyCode"": ""AUD"",
                    ""rate"": 14.343
                }]
                }")
            });

            var currencies = new List<Currency>() { new Currency("AUD")};
            var exchangeRates = await _cnbClient.GetCurrentExchangeRatesAsync(currencies);
            Assert.IsNotEmpty(exchangeRates);

        }
    }

    public class TestHttpMessageHandler : HttpMessageHandler
    {
        public Func<HttpRequestMessage, Task<HttpResponseMessage>> SendAsyncCallback;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsyncCallback(request);
        }
    }
}
