using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRate.Service.UnitTests.Extensions;
using Moq;
using Moq.Protected;

namespace ExchangeRate.Service.UnitTests.Mock;

public static class MockHttpClient
{
    public static Mock<IHttpClientFactory> GetMockHttpClientFactory(string content)
    {
        var httpClient = Create(new HttpResponseMessage(HttpStatusCode.OK).SetContent(content));
        var httpClientFactory = new Mock<IHttpClientFactory>();

        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        return httpClientFactory;
    }

    private static HttpClient Create(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        var httpClient = new HttpClient(handlerMock.Object);
        return httpClient;
    }
}