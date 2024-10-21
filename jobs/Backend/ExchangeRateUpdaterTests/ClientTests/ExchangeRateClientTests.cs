using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdaterTests.ClientTests
{
    public class ExchangeRateClientTests
    {
        Mock<ILogger<ExchangeRateClient>> _loggerMock;
        readonly Mock<IHttpClientFactory> _clientFactoryMock;
        readonly Mock<IRetryPolicy> _policyMock;
        readonly Mock<HttpClient> _httpClient;
        ExchangeRateClient _client;

        public ExchangeRateClientTests()
        {
            _loggerMock = new Mock<ILogger<ExchangeRateClient>>();
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _policyMock = new Mock<IRetryPolicy>();
            _httpClient = new Mock<HttpClient>();
            _client = new ExchangeRateClient(_clientFactoryMock.Object, _loggerMock.Object, _policyMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
			_loggerMock = new Mock<ILogger<ExchangeRateClient>>();
			_client = new ExchangeRateClient(_clientFactoryMock.Object, _loggerMock.Object, _policyMock.Object);
		}


        [Test]
        public void GivenResponseIsNull_WhenGetExchangeRateEntitiesAsyncIsCalled_EmptyCollectionIsReturned()
        {
            HttpResponseMessage? response = null;

            _clientFactoryMock.Setup(f => f.CreateClient("exchangeRates")).Returns(_httpClient.Object);
            _policyMock.Setup(p => p.ExecuteGetRequestWithRetry(It.IsAny<HttpClient>(), string.Empty, ExchangeRateSettings.MaxRetries, ExchangeRateSettings.RequestInterval)).ReturnsAsync(response);

            var result = _client.GetExchangeRateEntitiesAsync().Result;

            Assert.IsNotNull(result);
            Assert.That(result.Count() == 0);
        }

		[Test]
		public void GivenNotFoundResponse_WhenGetExchangeRateEntitiesAsyncIsCalled_EmptyCollectionIsReturned()
		{
			HttpResponseMessage? response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

			_clientFactoryMock.Setup(f => f.CreateClient("exchangeRates")).Returns(_httpClient.Object);
			_policyMock.Setup(p => p.ExecuteGetRequestWithRetry(It.IsAny<HttpClient>(), string.Empty, ExchangeRateSettings.MaxRetries, ExchangeRateSettings.RequestInterval)).ReturnsAsync(response);

			var result = _client.GetExchangeRateEntitiesAsync().Result;

			Assert.IsNotNull(result);
			Assert.That(result.Count() == 0);
		}

		[Test]
        public void GivenContentIsInvalid_WhenGetExchangeRateEntitiesAsyncIsCalled_EmptyCollectionIsReturned()
        {
            HttpResponseMessage response = new HttpResponseMessage
            {
                Content = null
            };

            _clientFactoryMock.Setup(f => f.CreateClient("exchangeRates")).Returns(_httpClient.Object);
            _policyMock.Setup(p => p.ExecuteGetRequestWithRetry(It.IsAny<HttpClient>(), string.Empty, ExchangeRateSettings.MaxRetries, ExchangeRateSettings.RequestInterval)).ReturnsAsync(response);

            var result = _client.GetExchangeRateEntitiesAsync().Result;

            Assert.IsNotNull(result);
            Assert.That(result.Count() == 0);
        }

        [Test]
        public void GivenContentIsInvalid_WhenGetExchangeRateEntitiesAsyncIsCalled_ExceptionIsLogged()
        {
            HttpResponseMessage response = new HttpResponseMessage
            {
                Content = null
            };

			_clientFactoryMock.Setup(f => f.CreateClient("exchangeRates")).Returns(_httpClient.Object);
            _policyMock.Setup(p => p.ExecuteGetRequestWithRetry(It.IsAny<HttpClient>(), string.Empty, ExchangeRateSettings.MaxRetries, ExchangeRateSettings.RequestInterval)).ReturnsAsync(response);

            var result = _client.GetExchangeRateEntitiesAsync().Result;

            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error while parsing content from Http response")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Test]
        public void GivenAValidContent_WhenGetExchangeRateEntitiesAsyncIsCalled_EntitiesAreReturned()
        {
            var jsonContent = @"{
				  ""rates"": [
					{
					  ""currencyCode"": ""AUD"",
					  ""rate"": 15.606
					},
					{
					  ""currencyCode"": ""BRL"",
					  ""rate"": 4.117
					},
					{
					  ""currencyCode"": ""BGN"",
					  ""rate"": 12.9
					}
				  ]
				}";

            HttpResponseMessage response = new HttpResponseMessage
            {
                Content = new StringContent(jsonContent)
            };

            _clientFactoryMock.Setup(f => f.CreateClient("exchangeRates")).Returns(_httpClient.Object);
            _policyMock.Setup(p => p.ExecuteGetRequestWithRetry(It.IsAny<HttpClient>(), string.Empty, ExchangeRateSettings.MaxRetries, ExchangeRateSettings.RequestInterval)).ReturnsAsync(response);

            var result = _client.GetExchangeRateEntitiesAsync().Result;

            Assert.IsNotNull(result);
            Assert.That(result.Count() == 3);
            Assert.That(result.Count(e => e.CurrencyCode == "AUD" && e.Rate == 15.606M) == 1);
            Assert.That(result.Count(e => e.CurrencyCode == "BRL" && e.Rate == 4.117M) == 1);
            Assert.That(result.Count(e => e.CurrencyCode == "BGN" && e.Rate == 12.9M) == 1);
        }
    }
}
