using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Service.Tests.Unit;

public static class TestHelper
{
    internal static HttpClient HttpClientWithResponse(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(response)
                   .Verifiable();

        return new HttpClient(handlerMock.Object);
    }
}