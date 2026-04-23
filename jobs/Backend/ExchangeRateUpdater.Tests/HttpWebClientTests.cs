using System.Net;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class HttpWebClientTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<HttpWebClient>> _mockLogger;
    private readonly SettingOptions _settings;
    private readonly HttpWebClient _client;

    public HttpWebClientTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_mockHttp);
        _mockLogger = new Mock<ILogger<HttpWebClient>>();

        _settings = new SettingOptions
        {
            BaseUrl = "https://api.cnb.cz",
            Endpoint = "daily.txt"
        };

        _client = new HttpWebClient(
            _httpClient,
            Options.Create(_settings),
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsSuccessResponse_WhenApiResponds()
    {
        // Arrange
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("test data")
        };

        _mockHttp.When($"{_settings.BaseUrl}/{_settings.Endpoint}")
                .Respond(req => expectedResponse);

        // Act
        var result = await _client.GetAsync();

        // Assert
        result.Should().BeSameAs(expectedResponse);
    }

    [Fact]
    public async Task GetAsync_ThrowsInvalidOperationException_WhenEndpointNotConfigured()
    {
        // Arrange
        var invalidClient = new HttpWebClient(
            _httpClient,
            Options.Create(new SettingOptions { BaseUrl = "https://api.cnb.cz" }), // Missing Endpoint
            _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => invalidClient.GetAsync());
        _mockLogger.VerifyLog(LogLevel.Error, "Endpoint configuration is missing or empty");
    }
    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenDependenciesNull()
    {
        // Arrange
        var validHttpClient = new HttpClient();
        var validSettings = Options.Create(new SettingOptions());
        var validLogger = Mock.Of<ILogger<HttpWebClient>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new HttpWebClient(null, validSettings, validLogger));

        Assert.Throws<NullReferenceException>(() =>
            new HttpWebClient(validHttpClient, null, validLogger));
    }

    [Fact]
    public void Constructor_SetsCorrectHeaders_WhenInitialized()
    {
        // Act (done in constructor)
        var acceptHeader = _httpClient.DefaultRequestHeaders.Accept.FirstOrDefault();

        // Assert
        acceptHeader.Should().NotBeNull();
        acceptHeader.MediaType.Should().Be("text/plain");
        _httpClient.BaseAddress.Should().Be(new Uri(_settings.BaseUrl));
    }
}

// Test helper extension
public static class LoggerTestExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times? times = null)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            times ?? Times.Once());
    }
}