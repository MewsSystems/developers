using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Lib.Shared;
using ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace ExchangeRateUpdate.CzechNationalBank.Test
{
    [TestFixture]
    public class ExchangeRateHttpClientTests
    {
        const string validResponseContent = "Currency: USD|Amount: 1\nDate|Rate\n01.07.2021|1.2\n02.07.2021|1.3";

        private IExchangeRateProviderSettings _settings;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private IExchangeRateHttpClient _exchangeRateHttpClient;
        private HttpClient _httpClient;
        private ILogger _logger;
        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleLogger(new ConsoleLoggerSettings());
            _settings = new ExchangeRateProviderSettings(
                sourceUrl: "http://example.com/rates?start={0}&end={1}&currency={2}",
                timeoutSeconds: 30,
                maxThreads: 8,
                rateLimitCount: 5,
                rateLimitDuration: 1,
                precision: 4
            );

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);


            _exchangeRateHttpClient = new ExchangeRateHttpClient(
                _settings,
                _httpClient,
                _logger
            );
        }

        [Test]
        public async Task GetCurrentExchangeRate_ShouldReturnExchangeRate_WhenResponseIsValid()
        {
            // Arrange
            var currency = new Currency("USD");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(validResponseContent)
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Act
            var exchangeRate = await _exchangeRateHttpClient.GetCurrentExchangeRateAsync(currency);

            // Assert
            Assert.IsNotNull(exchangeRate);
            Assert.AreEqual(currency, exchangeRate.Currency);
            Assert.AreEqual(2, exchangeRate.Rates.Count);
            Assert.AreEqual(1.2m, exchangeRate.Rates[new DateTime(2021, 7, 1)]);
            Assert.AreEqual(1.3m, exchangeRate.Rates[new DateTime(2021, 7, 2)]);
        }

        [Test]
        public async Task GetCurrentExchangeRate_ShouldReturnNull_WhenResponseContentIsEmpty()
        {
            // Arrange
            var currency = new Currency("USD");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Act
            var exchangeRate = await _exchangeRateHttpClient.GetCurrentExchangeRateAsync(currency);

            // Assert
            Assert.IsNull(exchangeRate);
        }

        [Test]
        public void Deserialize_ShouldThrowArgumentException_WhenDataIsInvalid()
        {
            // Arrange
            var invalidData = "Invalid data";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ProviderExchangeRate.Deserialize(invalidData));
        }

        [Test]
        public void Deserialize_ShouldReturnExchangeRate_WhenDataIsValid()
        {

            // Act
            var exchangeRate = ProviderExchangeRate.Deserialize(validResponseContent);

            // Assert
            Assert.IsNotNull(exchangeRate);
            Assert.AreEqual("USD", exchangeRate.Currency.Code);
            Assert.AreEqual(2, exchangeRate.Rates.Count);
            Assert.AreEqual(1.2m, exchangeRate.Rates[new DateTime(2021, 7, 1)]);
            Assert.AreEqual(1.3m, exchangeRate.Rates[new DateTime(2021, 7, 2)]);
        }
    }

}




