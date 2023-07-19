using System.Net;
using ExchangeRate.Core.Exceptions;
using ExchangeRate.Core.ExchangeRateSourceClients;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRate.Core.Tests.ExchangeRateSourceClients;

public class CnbExchangeRateClientTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactory;

    public CnbExchangeRateClientTests()
    {
        _httpClientFactory = new Mock<IHttpClientFactory>();
    }

    [Fact]
    public async void GetExchangeRatesAsync_UrlPathIsNull_ThrowsArgumentNullException()
    {
        var cnbExchangeRateClient = new CnbExchangeRateClient(_httpClientFactory.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await cnbExchangeRateClient.GetExchangeRatesAsync(null));
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "Bad request")]
    [InlineData(HttpStatusCode.Unauthorized, "Unauthorized request")]
    [InlineData(HttpStatusCode.Forbidden, "Forbidden to request")]
    [InlineData(HttpStatusCode.NotFound, "Not found resource")]
    [InlineData(HttpStatusCode.InternalServerError, "Internal server error")]
    public async void GetExchangeRatesAsync_ResponseIsNotSuccesfull_ThrowsExchangeSourceException(HttpStatusCode errorCode, string message)
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = errorCode,
                Content = new StringContent(message)
            });
        _httpClientFactory
            .Setup(f => f.CreateClient("CNB"))
            .Returns(new HttpClient(mockMessageHandler.Object));

        var cnbExchangeRateClient = new CnbExchangeRateClient(_httpClientFactory.Object);

        var exception = await Assert.ThrowsAsync<ExchangeRateSourceException>(async () => await cnbExchangeRateClient.GetExchangeRatesAsync("/test"));

        Assert.Equal(message, exception.Message);
    }
}
