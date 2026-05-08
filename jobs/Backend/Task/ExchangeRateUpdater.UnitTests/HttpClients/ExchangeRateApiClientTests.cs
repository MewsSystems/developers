using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Infrastructure.HttpClients;
using ExchangeRateUpdater.UnitTests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.HttpClients;

public class ExchangeRateApiClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
    private readonly Mock<ILogger<ExchangeRateApiClient>> _logger = new();
    private readonly IExchangeRateApiClient _exchangeRateApiClient;

    public ExchangeRateApiClientTests()
    {
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://test.com/")
        };
        
        _exchangeRateApiClient = new ExchangeRateApiClient(httpClient, _logger.Object);
    }

    [Fact]
    public async Task GetCnbExchangeRates_ApiReturnsCorrectData_Success()
    {
        // Arrange
        var expectedResponse =
            new CnbExchangeRateResponse([new CnbExchangeRate("USD", 20, 1), new CnbExchangeRate("EUR", 30, 1)]);
        _httpMessageHandlerMock
            .SetupGet("/exrates/daily")
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });
        
        // Act
        var result = await _exchangeRateApiClient.GetAllExchangeRatesAsync(CancellationToken.None);

        // Assert
        CnbExchangeRate[] expected = [new("USD", 20, 1), new("EUR", 30, 1)];
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task GetCnbExchangeRates_ApiReturnsEmptyCollection_LogsWarningReturnsEmptyCollection()
    {
        // Arrange
        var expectedResponse = new CnbExchangeRateResponse([]);
        _httpMessageHandlerMock
            .SetupGet("/exrates/daily")
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });
        
        // Act
        var result = await _exchangeRateApiClient.GetAllExchangeRatesAsync(CancellationToken.None);

        // Assert
        Assert.Equal([], result);
        _logger.VerifyLogged(LogLevel.Warning, "No exchange rates were provided by CNB");
    }
    
    [Fact]
    public async Task GetCnbExchangeRates_ApiThrowsException_LogsErrorThrowsException()
    {
        // Arrange
        _httpMessageHandlerMock
            .SetupGet("/exrates/daily")
            .ThrowsAsync(new HttpRequestException());
        
        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _exchangeRateApiClient.GetAllExchangeRatesAsync(CancellationToken.None));
        _logger.VerifyLogged(LogLevel.Error, "Http request to CNB has failed");
    }
}