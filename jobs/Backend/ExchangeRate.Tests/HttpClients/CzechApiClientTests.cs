using System.Net;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.HttpClients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace ExchangeRate.Tests.HttpClients
{
    public class CzechApiClientTests
    {
        private static CzechApiClient CreateClient(HttpResponseMessage response, string baseUrl = "https://api.cnb.cz")
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            var options = Options.Create(new CzechApiSettings { BaseUrl = baseUrl });
            var loggerMock = new Mock<ILogger<CzechApiClient>>();

            return new CzechApiClient(httpClient, options, loggerMock.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsContent_WhenSuccess()
        {
            // Arrange
            var expectedContent = "test-response";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(expectedContent)
            };
            var client = CreateClient(response);

            // Act
            var result = await client.GetAsync("/test-endpoint");

            // Assert
            Assert.Equal(expectedContent, result);
        }

        [Fact]
        public async Task GetAsync_ThrowsException_WhenNotSuccess()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var client = CreateClient(response);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => client.GetAsync("/fail-endpoint"));
        }

        [Fact]
        public async Task GetAsync_UsesBaseUrlAndRelativeUrl()
        {
            // Arrange
            var expectedBase = "https://api.cnb.cz";
            var expectedRelative = "/some-path";
            var expectedUrl = $"{expectedBase}{expectedRelative}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("ok")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var options = Options.Create(new CzechApiSettings { BaseUrl = expectedBase });
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<CzechApiClient>>();

            var client = new CzechApiClient(httpClient, options, loggerMock.Object);

            // Act
            var result = await client.GetAsync(expectedRelative);

            // Assert
            Assert.Equal("ok", result);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == expectedUrl),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            var options = Options.Create(new CzechApiSettings { BaseUrl = "https://api.cnb.cz" });
            var loggerMock = new Mock<ILogger<CzechApiClient>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CzechApiClient(null, options, loggerMock.Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenOptionsIsNull()
        {
            // Arrange
            var httpClient = new HttpClient();
            var loggerMock = new Mock<ILogger<CzechApiClient>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CzechApiClient(httpClient, null, loggerMock.Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
        {
            // Arrange
            var httpClient = new HttpClient();
            var options = Options.Create(new CzechApiSettings { BaseUrl = "https://api.cnb.cz" });

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CzechApiClient(httpClient, options, null));
        }
    }
}