using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests.Providers;

public class CNBExchangeRateApiProviderTests
{
    private readonly Mock<ILogger<CNBExchangeRateApiProvider>> _mockLogger;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CNBExchangeRateApiProviderTests()
    {
        _mockLogger = new Mock<ILogger<CNBExchangeRateApiProvider>>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        var settings = new Dictionary<string, string?>
        {
            ["CNBExchangeRateAPIProvider:ApiBaseUrl"] = "https://api.cnb.cz",
            ["CNBExchangeRateAPIProvider:DailyRatesEndpoint"] = "/cnbapi/exrates/daily",
            ["CNBExchangeRateAPIProvider:TargetCurrencyCode"] = "CZK"
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    #region Auxiliary Methods
    private CNBExchangeRateApiProvider CreateProvider()
        => new(_httpClient, _mockLogger.Object, _configuration);

    private void SetupHttpResponse(HttpStatusCode statusCode, string content)
    {
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            });
    }
    #endregion


    [Fact]
    public async Task GetExchangeRatesAsync_WithValidResponse_ReturnsExchangeRates()
    {
        var currencies = new List<Currency>
        {
            new("USD"),
            new("EUR")
        };

        var apiResponse = new
        {
            rates = new[]
            {
                new { currencyCode = "USD", amount = 1, rate = 20.367m },
                new { currencyCode = "EUR", amount = 1, rate = 24.220m },
                new { currencyCode = "JPY", amount = 100, rate = 13.044m }
            }
        };

        SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(apiResponse));

        var provider = CreateProvider();
        var result = await provider.GetExchangeRatesAsync(currencies);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.SourceCurrency.Code == "USD" && r.Value == 20.367m);
        Assert.Contains(result, r => r.SourceCurrency.Code == "EUR" && r.Value == 24.220m);
        Assert.DoesNotContain(result, r => r.SourceCurrency.Code == "JPY");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_NormalizesAmountCorrectly()
    {
        var currencies = new List<Currency> { new("JPY") };

        var apiResponse = new
        {
            rates = new[]
            {
                new { currencyCode = "JPY", amount = 100, rate = 13.044m }
            }
        };

        SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(apiResponse));

        var provider = CreateProvider();
        var result = await provider.GetExchangeRatesAsync(currencies);

        var rate = Assert.Single(result);
        Assert.Equal(0.13044m, rate.Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNoCurrencies_ReturnsEmpty()
    {
        var provider = CreateProvider();
        var result = await provider.GetExchangeRatesAsync([]);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithHttpError_ReturnsEmptyAndLogsError()
    {
        SetupHttpResponse(HttpStatusCode.InternalServerError, "Server error");

        var provider = CreateProvider();
        var result = await provider.GetExchangeRatesAsync([new Currency("USD")]);

        Assert.Empty(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to fetch exchange rates from CNB API")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidJson_ReturnsEmpty()
    {
        SetupHttpResponse(HttpStatusCode.OK, "invalid json {{{");

        var provider = CreateProvider();
        var result = await provider.GetExchangeRatesAsync([new Currency("USD")]);

        Assert.Empty(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to deserialize JSON response from CNB API")),
                It.IsAny<JsonException>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_IgnoresTargetCurrency()
    {
        var currencies = new[] { new Currency("USD"), new Currency("CZK") };

        var apiResponse = new
        {
            rates = new[]
            {
                new { currencyCode = "USD", amount = 1, rate = 20.367m },
                new { currencyCode = "CZK", amount = 1, rate = 1.0m }
            }
        };

        SetupHttpResponse(HttpStatusCode.OK, JsonSerializer.Serialize(apiResponse));

        var provider = CreateProvider();
        var result = await provider.GetExchangeRatesAsync(currencies);

        var rate = Assert.Single(result);
        Assert.Equal("USD", rate.SourceCurrency.Code);
    }
}
