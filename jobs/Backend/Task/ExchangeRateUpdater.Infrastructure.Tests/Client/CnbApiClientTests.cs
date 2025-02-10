using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.Infrastructure.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRateUpdater.Infrastructure.Tests.Client;

public class CnbApiClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<ILogger<CnbApiClient>> _loggerMock;
    private readonly Mock<IOptions<ApiSettings>> _apiSettingsMock;
    private readonly HttpClient _httpClient;
    private readonly CnbApiClient _cnbApiClient;

    public CnbApiClientTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _loggerMock = new Mock<ILogger<CnbApiClient>>();
        _apiSettingsMock = new Mock<IOptions<ApiSettings>>();

        _apiSettingsMock.Setup(x => x.Value).Returns(new ApiSettings { BaseUrl = "https://api.cnb.cz/" });

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.cnb.cz/")
        };

        _cnbApiClient = new CnbApiClient(_httpClient, _loggerMock.Object, _apiSettingsMock.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ReturnsValidData_WhenApiReturnsSuccess()
    {
        // Arrange
        var date = new DateTime(2024, 2, 10);
        var jsonResponse = """
        {
            "rates": [
                { "validFor": "2024-02-10", "currencyCode": "USD", "rate": 22.5, "amount": 1 },
                { "validFor": "2024-02-10", "currencyCode": "EUR", "rate": 24.3, "amount": 1 }
            ]
        }
        """;

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri!.ToString().Contains("cnbapi/exrates/daily")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _cnbApiClient.GetExchangeRatesAsync(date);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Rates.Count);
        Assert.Equal("USD", result.Rates[0].CurrencyCode);
        Assert.Equal(22.5m, result.Rates[0].Rate);
        Assert.Equal("EUR", result.Rates[1].CurrencyCode);
        Assert.Equal(24.3m, result.Rates[1].Rate);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsExternalServiceException_WhenApiReturnsError()
    {
        // Arrange
        var date = new DateTime(2024, 2, 10);
        var errorResponse = "Service Unavailable";

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.ServiceUnavailable,
            Content = new StringContent(errorResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExternalServiceException>(
            async () => await _cnbApiClient.GetExchangeRatesAsync(date));

        Assert.Contains("API returned", exception.Message);
        Assert.Contains("Service Unavailable", exception.Message);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsParsingException_WhenApiReturnsInvalidJson()
    {
        // Arrange
        var date = new DateTime(2024, 2, 10);
        var invalidJson = "{ invalid json }";

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(invalidJson)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act & Assert
        await Assert.ThrowsAsync<ParsingException>(
            async () => await _cnbApiClient.GetExchangeRatesAsync(date));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsExternalServiceException_WhenHttpRequestFails()
    {
        // Arrange
        var date = new DateTime(2024, 2, 10);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network failure"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExternalServiceException>(
            async () => await _cnbApiClient.GetExchangeRatesAsync(date));

        Assert.Contains("Failed to reach the external API", exception.Message);
    }
}
