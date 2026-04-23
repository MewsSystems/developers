using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net;
using Czech_National_Bank_ExchangeRates.Infrastructure;

namespace Czech_National_Bank_Exchange_Rates_Test
{
    [TestFixture]
    public class HttpClientServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClientService _service;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _service = new HttpClientService();
        }

        [Test]
        public async Task GetAsync_ErrorStatusCode_ReturnsDefault()
        {
            // Arrange
            var uri = "https://api.example.com/data";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == uri),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await _service.GetAsync<dynamic>(uri, string.Empty, string.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAsync_ExceptionThrown_ReturnsDefault()
        {
            // Arrange
            var uri = "https://api.example.com/data";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == uri),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _service.GetAsync<dynamic>(uri, string.Empty, string.Empty);

            // Assert
            Assert.IsNull(result);
        }
    }
}
