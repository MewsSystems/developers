using System.Net;
using ExchangeRateUpdater.Dto;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.ExternalServices;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using NSubstitute;
using Shouldly;
using ExchangeRateUpdater.Exceptions;

namespace ExchangeRateUpdater.Tests.ExternalServices;

public class CnbExchangeRateClientTests
{
    private const string TargetCurrencyCode = "CZK";
    private const string TestUrlPath = "fake-endpoint";
    private readonly IOptions<CnbExchangeRateOptions> _options;
    private readonly ILogger<CnbExchangeRateClient> _logger;
    public CnbExchangeRateClientTests()
    {
        CnbExchangeRateOptions cnbConfiguration = new()
        {
            BaseUrl = "https://url.com/",
            CommonUrl = "common",
            UncommonUrl = "uncommon"
        };
        _options = new OptionsWrapper<CnbExchangeRateOptions>(cnbConfiguration);
        _logger = Substitute.For<ILogger<CnbExchangeRateClient>>();
    }

    private static HttpClient CreateMockHttpClient(HttpResponseMessage httpResponseMessage, string baseUrl)
    {
        FakeHttpMessageHandler fakeHttpMessageHandler = new(httpResponseMessage);
        return new HttpClient(fakeHttpMessageHandler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    private static HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode statusCode, object? content = null)
    {
        HttpResponseMessage responseMessage = new()
        {
            StatusCode = statusCode
        };

        if (content != null)
        {
            responseMessage.Content = new StringContent(JsonSerializer.Serialize(content));
        }

        return responseMessage;
    }

    [Fact]
    public async Task FetchExchangeRatesAsync_ShouldReturnExchangeRates_WhenSuccess()
    {
        // Arrange
        CnbExchangeRate validCnbExchangeRate = new(DateOnly.Parse("2025-01-05"), 1, "United States", "dollar", 1, "USD", 1.23m);
        CnbExchangeRateResponseDto cnbExchangeRateResponseDto = new([
            validCnbExchangeRate
        ]);

        HttpResponseMessage httpResponseMessage = CreateHttpResponseMessage(HttpStatusCode.OK, cnbExchangeRateResponseDto);
        HttpClient httpClient = CreateMockHttpClient(httpResponseMessage, _options.Value.BaseUrl);

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = await sut.FetchExchangeRatesAsync(TestUrlPath);

        // Assert
        exchangeRates.ShouldNotBeNull();
        exchangeRates.Count().ShouldBe(1);
        ExchangeRate rate = exchangeRates.Single();

        rate.SourceCurrency.Code.ShouldBe(validCnbExchangeRate.CurrencyCode);
        rate.TargetCurrency.Code.ShouldBe(TargetCurrencyCode);
        rate.Value.ShouldBe(validCnbExchangeRate.Rate);
    }

    [Fact]
    public async Task FetchExchangeRatesAsync_ShouldThrowNoExchangeRatesReceived_WhenReturnedEmptyRates()
    {
        // Arrange
        CnbExchangeRateResponseDto cnbExchangeRateResponseDto = new([]);

        HttpResponseMessage httpResponseMessage = CreateHttpResponseMessage(HttpStatusCode.OK, cnbExchangeRateResponseDto);
        HttpClient httpClient = CreateMockHttpClient(httpResponseMessage, _options.Value.BaseUrl);

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        NoExchangeRatesReceivedException exception = await Should.ThrowAsync<NoExchangeRatesReceivedException>(sut.FetchExchangeRatesAsync(TestUrlPath));

        // Assert
        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("Fetching from CNB returned no exchange rates.")),
            Arg.Is<Exception>(ex => ex is NoExchangeRatesReceivedException),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task FetchExchangeRatesAsync_ShouldThrowHttpRequestException_WhenHttpRequestFails()
    {
        // Arrange
        HttpResponseMessage httpResponseMessage = CreateHttpResponseMessage(HttpStatusCode.InternalServerError);
        HttpClient httpClient = CreateMockHttpClient(httpResponseMessage, _options.Value.BaseUrl);

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        HttpRequestException exception = await Should.ThrowAsync<HttpRequestException>(sut.FetchExchangeRatesAsync(TestUrlPath));

        // Assert
        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("HTTP request failed when fetching rates from CNB")),
            Arg.Is<Exception>(ex => ex is HttpRequestException),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task FetchExchangeRatesAsync_ShouldThrowJsonException_WhenInvalidJsonReturned()
    {
        // Arrange
        HttpResponseMessage httpResponseMessage = CreateHttpResponseMessage(HttpStatusCode.OK, "This is invalid JSON");

        HttpClient httpClient = CreateMockHttpClient(httpResponseMessage, _options.Value.BaseUrl);

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        JsonException exception = await Should.ThrowAsync<JsonException>(sut.FetchExchangeRatesAsync(TestUrlPath));

        // Assert
        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("Error deserializing response from CNB.")),
            Arg.Is<Exception>(ex => ex is JsonException),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task FetchExchangeRatesAsync_ShouldThrow_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        HttpResponseMessage httpResponseMessage = CreateHttpResponseMessage(HttpStatusCode.OK);
        FakeHttpMessageHandlerWithException fakeHttpMessageHandler = new();
        HttpClient httpClient = new(fakeHttpMessageHandler)
        {
            BaseAddress = new Uri(_options.Value.BaseUrl)
        };

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        Exception exception = await Should.ThrowAsync<Exception>(sut.FetchExchangeRatesAsync(TestUrlPath));

        // Assert
        _logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("An unexpected error occurred while fetching rates from CNB.")),
            Arg.Is<Exception>(ex => ex is Exception),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void MapToExchangeRates_ShouldReturnMappedExchangeRates_WhenInputIsValid()
    {
        // Arrange
        HttpClient httpClient = Substitute.For<HttpClient>();

        CnbExchangeRate receivedUsdExchangeRate = new(DateOnly.Parse("2025-01-01"), 1, "United States", "Dollar", 1, "USD", 1.23m);
        CnbExchangeRate receivedEurExchangeRate = new(DateOnly.Parse("2025-01-01"), 1, "Netherlands", "Euro", 1, "EUR", 0.85m);
        IEnumerable<CnbExchangeRate> cnbRates =
        [
            receivedUsdExchangeRate,
            receivedEurExchangeRate
        ];

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = sut.MapToExchangeRates(cnbRates);

        // Assert
        exchangeRates.ShouldNotBeNull();
        exchangeRates.Count().ShouldBe(2);

        ExchangeRate? usdRate = exchangeRates.FirstOrDefault(r => r.SourceCurrency.Code == receivedUsdExchangeRate.CurrencyCode);
        usdRate.ShouldNotBeNull();
        usdRate.Value.ShouldBe(receivedUsdExchangeRate.Rate);
        usdRate.TargetCurrency.Code.ShouldBe(TargetCurrencyCode);

        ExchangeRate? eurRate = exchangeRates.FirstOrDefault(r => r.SourceCurrency.Code == receivedEurExchangeRate.CurrencyCode);
        eurRate.ShouldNotBeNull();
        eurRate.Value.ShouldBe(receivedEurExchangeRate.Rate);
        eurRate.TargetCurrency.Code.ShouldBe(TargetCurrencyCode);
    }

    [Fact]
    public void MapToExchangeRates_ShouldSkipRatesWithZeroValue()
    {
        // Arrange
        HttpClient httpClient = Substitute.For<HttpClient>();

        CnbExchangeRate invalidExchangeRate = new(DateOnly.Parse("2025-01-01"), 1, "Invalid Country", "Invalid Currency", 1, "XXX", 0m);
        IEnumerable<CnbExchangeRate> cnbRates =
        [
            new(DateOnly.Parse("2025-01-01"), 1, "United States", "Dollar", 1, "USD", 1.23m),
            invalidExchangeRate,
            new(DateOnly.Parse("2025-01-01"), 1, "Netherlands", "Euro", 1, "EUR", 0.85m)
        ];

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        IEnumerable<ExchangeRate> exchangeRates = sut.MapToExchangeRates(cnbRates);

        // Assert
        exchangeRates.ShouldNotBeNull();
        exchangeRates.Count().ShouldBe(2);
        exchangeRates.ShouldNotContain(r => r.SourceCurrency.Code == invalidExchangeRate.CurrencyCode);
        exchangeRates.ShouldNotContain(r => r.Value == invalidExchangeRate.Rate);

        _logger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Invalid Rate")),
            null,
            Arg.Any<Func<object, Exception?, string>>()
        );
    }

    [Theory]
    [InlineData(10, 1, 10)]
    [InlineData(10, 100, 0.1)]
    [InlineData(10, 1000, 0.01)]
    public void MapToExchangeRates_ShouldCalculateNormalizedRateCorrectly(decimal rate, int amount, decimal expectedNormalizedRate)
    {
        // Arrange
        HttpClient httpClient = Substitute.For<HttpClient>();

        IEnumerable<CnbExchangeRate> cnbRates =
        [
            new(DateOnly.Parse("2025-01-01"), 1, "Netherlands", "Euro", amount, "EUR", rate)
        ];

        CnbExchangeRateClient sut = new(_options, httpClient, _logger);

        // Act
        List<ExchangeRate> result = [.. sut.MapToExchangeRates(cnbRates)];

        // Assert
        result.Count.ShouldBe(1);
        result[0].Value.ShouldBe(expectedNormalizedRate);
    }
}
