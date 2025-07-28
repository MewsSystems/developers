using System.Net;
using System.Text;
using Mews.ExchangeRateUpdater.Infrastructure.Exceptions;
using Mews.ExchangeRateUpdater.Infrastructure.Interfaces;
using Mews.ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace Mews.ExchangeRateUpdater.Infrastructure.UnitTests.Services;

public class CnbServiceTests
{
    private const string JsonResponse = """
    {
        "rates": [
            { "validFor": "2025-07-25", "order": 143, "country": "USA", "currency": "dollar", "amount": 1, "currencyCode": "USD", "rate": 22.345 },
            { "validFor": "2025-07-25", "order": 143, "country": "EMU", "currency": "euro", "amount": 1, "currencyCode": "EUR", "rate": 24.123 }
        ]
    }
    """;

    private HttpClient CreateMockHttpClient(string content, HttpStatusCode status = HttpStatusCode.OK, string mediaType = "application/json")
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = status,
                Content = new StringContent(content, Encoding.UTF8, mediaType)
            });

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://dummy.local/")
        };
    }

    [Fact]
    public async Task ReturnsRates_WhenHttpSucceedsWithJson()
    {
        // Arrange
        var http = CreateMockHttpClient(JsonResponse);
        var logger = new Mock<ILogger<CnbService>>();
        var parser = new CnbParser(Mock.Of<ILogger<CnbParser>>());
        var service = new CnbService(http, parser, logger.Object);

        // Act
        var result = (await service.GetLatestRatesAsync(CancellationToken.None)).ToList();
        
        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.SourceCurrency.Code == "USD" && r.Value == 22.345m);
        Assert.Contains(result, r => r.SourceCurrency.Code == "EUR" && r.Value == 24.123m);
    }

    [Fact]
    public async Task ReturnsEmptyList_WhenJsonRatesAreEmpty()
    {
        // Arrange
        var emptyJson = """{ "rates": [] }""";
        var http = CreateMockHttpClient(emptyJson);
        var parser = new CnbParser(Mock.Of<ILogger<CnbParser>>());
        var service = new CnbService(http, parser, Mock.Of<ILogger<CnbService>>());
        
        // Act
        var result = await service.GetLatestRatesAsync(CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task ThrowsCnbClientException_OnHttpFailure()
    {
        // Arrange
        var http = CreateMockHttpClient("", HttpStatusCode.InternalServerError);
        var parser = new CnbParser(Mock.Of<ILogger<CnbParser>>());
        var service = new CnbService(http, parser, Mock.Of<ILogger<CnbService>>());
        
        // Act & Assert
        await Assert.ThrowsAsync<CnbServiceException>(() => service.GetLatestRatesAsync(CancellationToken.None));
    }

    [Fact]
    public async Task ThrowsCnbClientException_WhenDeserializationFails()
    {
        // Arrange
        var invalidJson = """{ "rates": [ { "currencyCode": "USD", "rate": "invalid" } ] }""";
        var http = CreateMockHttpClient(invalidJson);
        var parser = new CnbParser(Mock.Of<ILogger<CnbParser>>());
        var service = new CnbService(http, parser, Mock.Of<ILogger<CnbService>>());
        
        // Act & Assert
        var ex = await Assert.ThrowsAsync<CnbServiceException>(() =>
            service.GetLatestRatesAsync(CancellationToken.None));
        Assert.NotNull(ex.InnerException); // Should wrap deserialization exception
    }
}