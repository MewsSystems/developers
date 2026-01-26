using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models.CzechNationalBank;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateUpdaterTests.Infrastructure;

public class CzechNationalBankExchangeRateClientTests
{
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private readonly Mock<ILogger<CzechNationalBankExchangeRateClient>> _loggerMock;
    private readonly CzechNationalBankExchangeRateClient _sut;

    public CzechNationalBankExchangeRateClientTests()
    {
        var optionsMock = new Mock<IOptions<CzechNationalBankClientOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new CzechNationalBankClientOptions
        {
            RetryCount = 2,
            TimeoutMs = 10
        });

        _handlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("https://test.com/")
        };

        _loggerMock = new Mock<ILogger<CzechNationalBankExchangeRateClient>>();
        _sut = new CzechNationalBankExchangeRateClient(optionsMock.Object, httpClient, _loggerMock.Object);
    }

    [Fact]
    public async Task Given_SuccessfulResponse_When_RetrieveExchangeRatesAsync_Then_ReturnsResponse()
    {
        // Arrange
        var expectedResponse = new Response(new List<RecordedRate>
        {
            new(1, "USA", "Dollar", "USD", 1, 25.125m, DateTime.Now)
        });

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedResponse)
            });

        var result = await _sut.RetrieveExchangeRatesAsync();
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankExchangeRateClient_RetrievedResponse")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        var output = result.Rates.First();
        output.Should().Be(expectedResponse.Rates.First());
    }

    [Fact]
    public async Task Given_UnsuccessfulStatusCode_When_RetrieveExchangeRatesAsync_Then_ReturnsEmptyResponseAndLogsError()
    {
        var errorResponse = new ErrorResponse("Error", "/test", "404", DateTime.Now, "123");

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = JsonContent.Create(errorResponse)
            });

        var result = await _sut.RetrieveExchangeRatesAsync();

        result.Should().BeOfType<EmptyResponse>();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankExchangeRateClient_UnsuccessfulResponse")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankExchangeRateClient_RetrievedResponse")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_HttpRequestException_When_RetrieveExchangeRatesAsync_Then_ReturnsEmptyResponseAndLogsError()
    {
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var result = await _sut.RetrieveExchangeRatesAsync();

        result.Should().BeOfType<EmptyResponse>();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankExchangeRateClient_UnexpectedException")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Given_JsonException_When_RetrieveExchangeRatesAsync_Then_ReturnsEmptyResponseAndLogsError()
    {
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new JsonException("Error"));

        var result = await _sut.RetrieveExchangeRatesAsync();

        result.Should().BeOfType<EmptyResponse>();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => 
                    v.ToString()!.Contains("CzechNationalBankExchangeRateClient_DeserializationException") &&
                    v.ToString()!.Contains("Error")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Given_TimeoutException_When_RetrieveExchangeRatesAsync_Then_ReturnsEmptyResponseAndLogsError()
    {
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("Timeout!"));

        var result = await _sut.RetrieveExchangeRatesAsync();

        result.Should().BeOfType<EmptyResponse>();
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankExchangeRateClient_RetryAttempt_2")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => 
                    v.ToString()!.Contains("CzechNationalBankExchangeRateClient_Timeout") &&
                    v.ToString()!.Contains("Timeout!")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
