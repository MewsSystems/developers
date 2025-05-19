using System.Net;
using ExchangeRateUpdater.DataFetchers;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Moq.Protected;
using Polly;
using Polly.Retry;

namespace ExchangeRateUpdater.UnitTests
{
    [TestClass]
    public class HttpDataFetcherTests
    {
        #region Test Data Fields
        private readonly string _methodName = "SendAsync";

        private readonly string _expectedContent = "15 May 2025 #92\r\nCountry|Currency|Amount|Code|Rate\r\nAustralia|dollar|1|AUD|14.281\r\nBrazil|real|1|BRL|3.960";

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy = Policy
               .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        #endregion

        [TestMethod]
        public void FetchData_WithSuccessfulReponse_ReturnsExpectedData()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(_methodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(_expectedContent)
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var fetcher = new HttpDataFetcher(httpClient, _retryPolicy);

            // Act
            var result = fetcher.FetchData();

            // Assert
            result.Should().Be(_expectedContent);
        }

        [TestMethod]
        public void FetchData_WithFailures_RetriesAndReturnsExpectedData()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            int callCount = 0;

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(_methodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    callCount++;
                    if (callCount < 3)
                    {
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(_expectedContent)
                    };
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var fetcher = new HttpDataFetcher(httpClient, _retryPolicy);

            // Act
            var result = fetcher.FetchData();

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(_expectedContent);
                mockHandler.Protected().Verify(_methodName, Times.Exactly(3), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            }
                   
        }

        [TestMethod]
        public void FetchData_WithNotSuccessfulReponse_ThrowsExceptionAfterMaxRetries()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(_methodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));


            var httpClient = new HttpClient(mockHandler.Object);
            var fetcher = new HttpDataFetcher(httpClient, _retryPolicy);

            // Act
            Action act = () => fetcher.FetchData();

            // Assert
            using (new AssertionScope())
            {
                act.Should().Throw<HttpRequestException>();
                mockHandler.Protected().Verify(_methodName, Times.Exactly(4), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            }
        }

        [TestMethod]
        public void FetchData_WithCanceledTask_ThrowsTaskCanceledException()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(_methodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException());

            var httpClient = new HttpClient(mockHandler.Object);
            var fetcher = new HttpDataFetcher(httpClient, _retryPolicy);

            // Act
            Action act = () => fetcher.FetchData();

            // Assert
            act.Should().Throw<TaskCanceledException>();
        }
    }
}
