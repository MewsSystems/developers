using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Serilog;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateServiceTests
    {
        private const string ExchangeRateUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        [Fact]
        public async Task FetchExchangeRateDataAsync_ShouldReturnData_WhenResponseIsSuccessful()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req =>
                     req.Method == HttpMethod.Get &&
                     req.RequestUri == new Uri(ExchangeRateUrl)),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("Test data")
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new CnbExchangeRateService
            {
                HttpClient = httpClient,
                Logger = mockLogger.Object
            };

            // Act
            var result = await service.FetchExchangeRateDataAsync();

            // Assert
            Assert.Equal("Test data", result);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get &&
                  req.RequestUri == new Uri(ExchangeRateUrl)),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task FetchExchangeRateDataAsync_ShouldLogErrorAndThrowException_WhenRequestFails()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req =>
                     req.Method == HttpMethod.Get &&
                     req.RequestUri == new Uri(ExchangeRateUrl)),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.InternalServerError,
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new CnbExchangeRateService
            {
                HttpClient = httpClient,
                Logger = mockLogger.Object
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.FetchExchangeRateDataAsync());

            Assert.Equal("Error fetching exchange rate data from CNB", exception.Message);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get &&
                  req.RequestUri == new Uri(ExchangeRateUrl)),
               ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
