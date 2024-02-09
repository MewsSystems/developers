using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ExchangeRateApi.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private readonly HttpClient _client;
        private readonly HttpResponseMessage _responseMessage;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public ExchangeRateProviderTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _client = new HttpClient(_mockHttpMessageHandler.Object);
            _responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(""),
            };

            // Setup mock configuration
            _mockConfiguration.Setup(c => c["ExchangeRateProvider:BaseUrl"]).Returns("http://testapi.com");

            // Setup mocked HttpMessageHandler
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(_responseMessage);

            // Setup HttpClientFactory to return the mocked HttpClient
            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_client);
        }

        [Fact]
        public async Task GetExchangeRate_SuccessfulRetrieval_ReturnsExchangeRate()
        {
            // Arrange
            var expectedRate = new ExchangeRate { CurrencyCode = "USD", Rate = 1.0m };
            var jsonResponse = JsonSerializer.Serialize(new { Rates = new[] { expectedRate } });
            _responseMessage.Content = new StringContent(jsonResponse);
            _responseMessage.StatusCode = HttpStatusCode.OK;
            var provider = new ExchangeRateProvider(_mockHttpClientFactory.Object, _mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await provider.GetExchangeRate("USD");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRate.CurrencyCode, result.CurrencyCode);
            Assert.Equal(expectedRate.Rate, result.Rate);
        }

        [Fact]
        public async Task GetExchangeRate_NonSuccessfulHttpResponse_ThrowsExchangeRateApiException()
        {
            // Arrange
            _responseMessage.StatusCode = HttpStatusCode.BadRequest;
            var provider = new ExchangeRateProvider(_mockHttpClientFactory.Object, _mockConfiguration.Object, _mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateApiException>(() => provider.GetExchangeRate("USD"));
        }

        [Fact]
        public async Task GetExchangeRate_InvalidCurrencyCode_ThrowsCurrencyNotFoundException()
        {
            // Arrange
            var jsonResponse = JsonSerializer.Serialize(new { Rates = new ExchangeRate[] { } });
            _responseMessage.Content = new StringContent(jsonResponse);
            _responseMessage.StatusCode = HttpStatusCode.OK;
            var provider = new ExchangeRateProvider(_mockHttpClientFactory.Object, _mockConfiguration.Object, _mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<CurrencyNotFoundException>(() => provider.GetExchangeRate("INVALID"));
        }

        [Fact]
        public async Task GetExchangeRate_CorrectExchangeRate_ReturnsExpectedRate()
        {
            // Arrange
            var expectedRate = new ExchangeRate { CurrencyCode = "EUR", Rate = 0.85m };
            var jsonResponse = JsonSerializer.Serialize(new { Rates = new[] { expectedRate } });
            _responseMessage.Content = new StringContent(jsonResponse);
            _responseMessage.StatusCode = HttpStatusCode.OK;
            var provider = new ExchangeRateProvider(_mockHttpClientFactory.Object, _mockConfiguration.Object, _mockLogger.Object);

            // Act
            var result = await provider.GetExchangeRate("EUR");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRate.CurrencyCode, result.CurrencyCode);
            Assert.Equal(expectedRate.Rate, result.Rate);
        }

    }
}
