using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.UnitTest.Fixture
{
    public static class HttpClientMock
    {
        public static HttpClient GetMockHttpClient(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://mock-base-address.com/api")
            };
        }

        public static HttpClient GetMockHttpClientThrowsException(Exception exception)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(exception);

            return new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://mock-base-address.com/api")
            };
        }
    }
}
