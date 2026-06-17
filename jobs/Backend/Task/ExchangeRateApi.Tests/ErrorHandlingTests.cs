using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ExchangeRateApi.Tests
{
    public class ErrorHandlingTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private readonly Mock<IConfiguration> _configuration;

        public ErrorHandlingTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _configuration = new Mock<IConfiguration>();

            _configuration.Setup(c => c["ExchangeRateProvider:BaseUrl"]).Returns("http://testapi.com");
        }

        [Fact]
        public async Task GetExchangeRate_WhenApiCallFails_LogsErrorAndThrows()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(""),
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var provider = new ExchangeRateProvider(_mockHttpClientFactory.Object, _configuration.Object, _mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateApiException>(() => provider.GetExchangeRate("USD"));
            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("error")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
