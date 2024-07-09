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
        private static DateTime _lastRequestTime = DateTime.MinValue;
        private static readonly object _lock = new object();

        private async Task RateLimit()
        {
            lock (_lock)
            {
                var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
                if (timeSinceLastRequest < TimeSpan.FromSeconds(1))
                {
                    var delay = TimeSpan.FromSeconds(1) - timeSinceLastRequest;
                    Thread.Sleep(delay);
                }
                _lastRequestTime = DateTime.UtcNow;
            }
        }

        [Fact]
        public async Task FetchExchangeRateDataAsync_ShouldReturnData_WhenResponseIsSuccessful()
        {
            // Arrange
            await RateLimit();

            var mockLogger = new Mock<ILogger>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req =>
                     req.Method == HttpMethod.Get &&
                     req.RequestUri == new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("Test data")
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://www.cnb.cz/")
            };

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
                  req.RequestUri == new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")),
               ItExpr.IsAny<CancellationToken>()
            );
        }


        [Fact]
        public async Task FetchExchangeRateDataAsync_ShouldLogErrorAndThrowException_WhenRequestFails()
        {
            // Arrange
            await RateLimit();

            var mockLogger = new Mock<ILogger>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req =>
                     req.Method == HttpMethod.Get &&
                     req.RequestUri == new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.InternalServerError,
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://www.cnb.cz/")
            };

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
                  req.RequestUri == new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")),
               ItExpr.IsAny<CancellationToken>()
            );

            mockLogger.Verify(
               x => x.Error(It.IsAny<HttpRequestException>(), It.IsAny<string>(), It.IsAny<object[]>()),
               Times.Once);
        }
    }
}
