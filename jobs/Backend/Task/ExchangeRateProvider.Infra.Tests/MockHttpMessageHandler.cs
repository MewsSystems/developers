using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace ExchangeRateProvider.Infra.Tests;

public class MockHttpMessageHandler : Mock<HttpMessageHandler>
{
    public void SetupResponse(HttpResponseMessage response)
    {
        this.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response)
            .Verifiable();
    }
}