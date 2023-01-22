using Moq;
using Moq.Protected;
using System.Net;

namespace Tests.Helpers;

public class HttpClientHelper
{
    public static Mock<HttpMessageHandler> GetHttpMessageHandlerMock<T>(T response)
    {
        var mockResponse = new HttpResponseMessage()
        {
            Content = new StringContent(response?.ToString() ?? string.Empty),
            StatusCode = HttpStatusCode.OK
        };

        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler
        .Protected()
        .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(mockResponse);

        return mockHandler;
    }
}