using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Models.External;
using ExchangeRateUpdater.Tests.Services.TestHelper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ExchangeRateUpdater.Tests.Services;

public class CnbApiClientTests
{
    private readonly HttpClient _httpClient;
    private readonly FakeHttpMessageHandler _httpHandler;
    private readonly ApiConfiguration _config;
    private readonly TestLogger<CnbApiClient> _logger;
    private readonly IDateTimeSource _dateTimeSource;

    public CnbApiClientTests()
    {
        _httpHandler = new FakeHttpMessageHandler();
        _httpClient = new HttpClient(_httpHandler);

        _config = new ApiConfiguration
        {
            Language = "en",
            ExchangeRateEndpoint = "https://api.example.com/rates"
        };

        _logger = new TestLogger<CnbApiClient>();
        _dateTimeSource = A.Fake<IDateTimeSource>();
        A.CallTo(() => _dateTimeSource.UtcNow)
            .Returns(new DateTime(2025, 10, 27));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ReturnsRates_WhenResponseIsValid()
    {
        var expectedRates = new[]
        {
            new CnbRate ("USD", 1.0m, 1),
            new CnbRate ("EUR", 0.9m, 1)
        };

        var response = new CnbExchangeResponse(expectedRates);
        _httpHandler.SetResponse(response);

        var client = new CnbApiClient(_httpClient, _config, _logger, _dateTimeSource);

        var result = await client.GetExchangeRatesAsync();

        result.Should().BeEquivalentTo(expectedRates);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenRatesAreNull()
    {
        var response = new CnbExchangeResponse(null);
        _httpHandler.SetResponse(response);

        var client = new CnbApiClient(_httpClient, _config, _logger, _dateTimeSource);

        var result = await client.GetExchangeRatesAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsException_WhenHttpFails()
    {
        _httpHandler.SetException(new HttpRequestException("Network error"));

        var client = new CnbApiClient(_httpClient, _config, _logger, _dateTimeSource);

        Func<Task> act = async () => await client.GetExchangeRatesAsync();

        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("Network error");

        _logger.LogMessages.Should().Contain(m =>
            m.Message.Contains("An error occurred")
            && m.LogLevel == LogLevel.Error);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_LogsRequestUri()
    {
        var response = new CnbExchangeResponse([]);
        _httpHandler.SetResponse(response);

        var client = new CnbApiClient(_httpClient, _config, _logger, _dateTimeSource);

        await client.GetExchangeRatesAsync();

        _logger.LogMessages.Should().Contain(m =>
            m.Message.Contains("https://api.example.com/rates")
            && m.LogLevel == LogLevel.Information);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsJsonException_WhenResponseIsMalformed()
    {
        _httpHandler.SetRawResponse("not valid json");

        var client = new CnbApiClient(_httpClient, _config, _logger, _dateTimeSource);

        var act = client.GetExchangeRatesAsync;

        await act.Should().ThrowAsync<JsonException>();

        _logger.LogMessages.Should().Contain(m =>
            m.Message.Contains("An error occurred")
            && m.LogLevel == LogLevel.Error);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ThrowsTaskCanceledException_WhenRequestTimesOut()
    {
        _httpHandler.SetDelayedResponse(TimeSpan.FromSeconds(10));
        _httpClient.Timeout = TimeSpan.FromMilliseconds(100); // Force timeout

        var client = new CnbApiClient(_httpClient, _config, _logger, _dateTimeSource);

        var act = client.GetExchangeRatesAsync;

        await act.Should().ThrowAsync<TaskCanceledException>();

        _logger.LogMessages.Should().Contain(m =>
            m.Message.Contains("An error occurred")
            && m.LogLevel == LogLevel.Error);
    }
}
