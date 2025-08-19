using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Services.Clients;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

namespace ExchangeRateUpdater.Tests.Clients;

public class CnbApiClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<CnbApiClient>> _loggerMock;
    private readonly CnbApiConfiguration _configuration;
    private readonly CnbApiClient _client;

    public CnbApiClientTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://test.cnb.cz/")
        };
        _loggerMock = new Mock<ILogger<CnbApiClient>>();
        _configuration = new CnbApiConfiguration
        {
            CnbDailyRatesUrl = "https://test.cnb.cz/daily.txt",
            TimeoutSeconds = 30,
            RetryCount = 3
        };

        var options = Options.Create(_configuration);
        _client = new CnbApiClient(_httpClient, _loggerMock.Object, options);
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_WithSuccessfulResponse_ReturnsContent()
    {
        // Arrange
        var expectedContent = @"19 Aug 2025 #159
                                Country|Currency|Amount|Code|Rate
                                Australia|dollar|1|AUD|14.165";

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(expectedContent)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetExchangeRateDataAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedContent);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Retrieved") && v.ToString().Contains("characters")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_WithHttpError_ReturnsNetworkError()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError,
            ReasonPhrase = "Internal Server Error"
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetExchangeRateDataAsync();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.NetworkError);
        result.Errors[0].Message.Should().Contain("API returned InternalServerError");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("CNB API returned")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_WithTimeout_ReturnsTimeoutError()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("The operation was canceled."));

        // Act
        var result = await _client.GetExchangeRateDataAsync();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.TimeoutError);
        result.Errors[0].Message.Should().Contain($"Request timed out after {_configuration.TimeoutSeconds} seconds");
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_WithHttpRequestException_ReturnsNetworkError()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act
        var result = await _client.GetExchangeRateDataAsync();

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Metadata["ErrorCode"].Should().Be(CnbErrorCode.NetworkError);
        result.Errors[0].Message.Should().Be("Network error occurred");
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_UsesCorrectUrl()
    {
        // Arrange
        HttpRequestMessage capturedRequest = null;
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("test content")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(response);

        // Act
        await _client.GetExchangeRateDataAsync();

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Get);
        capturedRequest.RequestUri.ToString().Should().Be(_configuration.CnbDailyRatesUrl);
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_WithCancellation_PropagatesCancellation()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.Is<CancellationToken>(ct => ct.IsCancellationRequested))
            .ThrowsAsync(new OperationCanceledException());

        // Act
        var result = await _client.GetExchangeRateDataAsync(cts.Token);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task GetExchangeRateDataAsync_WithEmptyResponse_ReturnsSuccessWithEmptyString()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetExchangeRateDataAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithNullDependencies_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new CnbApiClient(null, _loggerMock.Object, Options.Create(_configuration)));

        Assert.Throws<ArgumentNullException>(() =>
            new CnbApiClient(_httpClient, null, Options.Create(_configuration)));

        Assert.Throws<ArgumentNullException>(() =>
            new CnbApiClient(_httpClient, _loggerMock.Object, null));
    }
}
