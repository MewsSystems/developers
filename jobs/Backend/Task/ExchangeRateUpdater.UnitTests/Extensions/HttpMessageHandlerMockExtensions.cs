using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace ExchangeRateUpdater.UnitTests.Extensions;

public static class HttpMessageHandlerMockExtensions
{
    public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> 
        SetupGet(this Mock<HttpMessageHandler> handler, string relativeUrl)
    {
        return handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.AbsolutePath == relativeUrl),
                ItExpr.IsAny<CancellationToken>());
    }
}