using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Helpers;
using ExchangeRateUpdater.Domain.Model.Cnb.Rq;
using ExchangeRateUpdater.Domain.Model.Cnb.Rs;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRateUpdater.Test
{
    public class HttpClientServiceTests
    {
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> httpHandlerMock;
        private readonly Mock<ILogger<HttpClientService>> loggerMock;
        private readonly IOptions<PollyConfig> pollyConfigMock;

        public HttpClientServiceTests()
        {
            httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpHandlerMock = new Mock<HttpMessageHandler>();

            pollyConfigMock = Options.Create(new PollyConfig
            {
                RetryCountAttempts = 3,
                SleepRetrySeconds = 1
            });

            loggerMock = new Mock<ILogger<HttpClientService>>();
        }

        private void SetupHttpClientFactoryMock(HttpStatusCode statusCode)
        {
            httpHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(""),
                });

            var httpClient = new HttpClient(httpHandlerMock.Object)
            {
                BaseAddress = new Uri("https://test.com"),
            };

            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }

        [Fact]
        public async Task GetDataAsync_WithInvalidUrl_ReturnsHandledError()
        {
            SetupHttpClientFactoryMock(HttpStatusCode.OK);

            var service = new HttpClientService(httpClientFactoryMock.Object, pollyConfigMock, loggerMock.Object);

            var result = await service.GetAsync<CnbExchangeRatesRsModel>("","");

            Assert.NotNull(result);
            Assert.False(result.Success);

            httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );

            var message = "Missing data on api call";
            VerifyLoggerMessage(message, LogLevel.Error);
            Assert.Equal(message, result.Message);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadRequest)]
        public async Task GetDataAsync_WithDifferentResponseStatusCode_ReturnsHandledError(HttpStatusCode responseCodeMock)
        {
            SetupHttpClientFactoryMock(responseCodeMock);

            var clientName = "test";

            var service = new HttpClientService(httpClientFactoryMock.Object, pollyConfigMock, loggerMock.Object);

            var model = new CnbExchangeRatesRqModel("EN");
            var uri = UrlHelper.GetUriFromModelWithParams("/success", model);

            var result = await service.GetAsync<CnbExchangeRatesRsModel>(clientName, uri);

            Assert.NotNull(result);
            Assert.False(result.Success);

            httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );

            var message = $"API call to '{clientName}' '{uri}' failed with status {responseCodeMock}. Response: ";
            VerifyLoggerMessage(message, LogLevel.Error);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public async Task GetDataAsync_WithRequestTimeOutAndRetries_ReturnsHandledError()
        {
            SetupHttpClientFactoryMock(HttpStatusCode.RequestTimeout);

            var clientName = "test";

            var service = new HttpClientService(httpClientFactoryMock.Object, pollyConfigMock, loggerMock.Object);

            var model = new CnbExchangeRatesRqModel("EN");
            var uri = UrlHelper.GetUriFromModelWithParams("/success", model);

            var result = await service.GetAsync<CnbExchangeRatesRsModel>(clientName, uri);

            Assert.NotNull(result);
            Assert.False(result.Success);

            httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(4),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true), 
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Exactly(3));

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }


        private void VerifyLoggerMessage(string message, LogLevel logLevel = LogLevel.Warning)
        {
            loggerMock.Verify(
                x => x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}