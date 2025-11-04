using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.RateProviders;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests.Services.RateProviders;

[Trait("Category", "Unit")]
public class CzechNationalBankRestApiExchangeRateProviderTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly Mock<IAppConfiguration> _mockConfig;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<ILogger<CzechNationalBankRestApiExchangeRateProvider>> _mockLogger;
    private readonly List<Currency> _multipleRequestedCurrencies;
    private readonly Currency _usdCurrency;
    private readonly string _validJsonResponse;

    public CzechNationalBankRestApiExchangeRateProviderTests()
    {
        _mockLogger = new Mock<ILogger<CzechNationalBankRestApiExchangeRateProvider>>();
        _mockConfig = new Mock<IAppConfiguration>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        const string testBaseUrl = "https://api.cnb.cz";
        _mockConfig.Setup(c => c.DailyRateUrl).Returns(testBaseUrl);
        _mockConfig.Setup(c => c.CzkCurrencyCode).Returns("CZK");

        _usdCurrency = new Currency("USD");
        var eurCurrency = new Currency("EUR");
        var gbpCurrency = new Currency("GBP");

        _multipleRequestedCurrencies = new List<Currency> { _usdCurrency, eurCurrency, gbpCurrency };

        var validResponse = new ExchangeRateResponseDto
        {
            Rates = new List<RateDto>
            {
                new() { CurrencyCode = "USD", Amount = 1, Rate = 22.500m, ValidFor = "2025-10-01" },
                new() { CurrencyCode = "EUR", Amount = 1, Rate = 25.000m, ValidFor = "2025-10-01" },
                new() { CurrencyCode = "GBP", Amount = 1, Rate = 30.000m, ValidFor = "2025-10-01" },
                new() { CurrencyCode = "AUD", Amount = 1, Rate = 15.234m, ValidFor = "2025-10-01" }
            }
        };

        _validJsonResponse = JsonSerializer.Serialize(validResponse);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private CzechNationalBankRestApiExchangeRateProvider CreateProvider()
    {
        return new CzechNationalBankRestApiExchangeRateProvider(_mockLogger.Object, _mockConfig.Object, _httpClient);
    }

    private void SetupHttpResponse(string content, HttpStatusCode statusCode = HttpStatusCode.OK)
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

    [Fact]
    public void Constructor_WithValidDependencies_CreatesInstance()
    {
        // Arrange & Act
        var provider = CreateProvider();

        // Assert
        Assert.NotNull(provider);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithEmptyCurrencyList_ReturnsEmptyList()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var emptyCurrencies = Enumerable.Empty<Currency>();

        // Act
        var result = await provider.GetExchangeRatesAsync(emptyCurrencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithSingleCurrency_ReturnsSingleRate()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Single(rates);
        Assert.Equal("USD", rates[0].SourceCurrency.Code);
        Assert.Equal("CZK", rates[0].TargetCurrency.Code);
        Assert.Equal(22.500m, rates[0].Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithMultipleCurrencies_ReturnsAllMatchingRates()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();

        // Act
        var result = await provider.GetExchangeRatesAsync(_multipleRequestedCurrencies);

        // Assert
        var rates = result.ToList();
        Assert.Equal(3, rates.Count);
        Assert.Contains(rates, r => r.SourceCurrency.Code == "USD" && r.Value == 22.500m);
        Assert.Contains(rates, r => r.SourceCurrency.Code == "EUR" && r.Value == 25.000m);
        Assert.Contains(rates, r => r.SourceCurrency.Code == "GBP" && r.Value == 30.000m);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNonExistentCurrency_FiltersOutNonMatching()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency, new("XXX") };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Single(rates);
        Assert.Equal("USD", rates[0].SourceCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_NormalizesRatesByAmount()
    {
        // Arrange
        var responseWithAmount = new ExchangeRateResponseDto
        {
            Rates = new List<RateDto>
            {
                new() { CurrencyCode = "JPY", Amount = 100, Rate = 15.000m, ValidFor = "2025-10-01" }
            }
        };
        SetupHttpResponse(JsonSerializer.Serialize(responseWithAmount));
        var provider = CreateProvider();
        var currencies = new List<Currency> { new("JPY") };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Single(rates);
        Assert.Equal(0.15m, rates[0].Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_ParsesDateCorrectly()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        var expectedDate = DateOnly.Parse("2025-10-01");
        Assert.Equal(expectedDate, rates[0].Date);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithCaseInsensitiveCurrencyCode_FindsMatch()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { new("usd") };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Single(rates);
        Assert.Equal("USD", rates[0].SourceCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNullRates_ReturnsEmptyList()
    {
        // Arrange
        var responseWithNullRates = new ExchangeRateResponseDto { Rates = null };
        SetupHttpResponse(JsonSerializer.Serialize(responseWithNullRates));
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNullRates_LogsWarning()
    {
        // Arrange
        var responseWithNullRates = new ExchangeRateResponseDto { Rates = null };
        SetupHttpResponse(JsonSerializer.Serialize(responseWithNullRates));
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        await provider.GetExchangeRatesAsync(currencies);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No exchange rates found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithEmptyRatesList_ReturnsEmptyList()
    {
        // Arrange
        var responseWithEmptyRates = new ExchangeRateResponseDto { Rates = new List<RateDto>() };
        SetupHttpResponse(JsonSerializer.Serialize(responseWithEmptyRates));
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_LogsDebugMessages()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        await provider.GetExchangeRatesAsync(currencies);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Fetching exchange rates")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_LogsSkippedCurrencies()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        await provider.GetExchangeRatesAsync(currencies);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("is not in the requested list")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(3));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_LogsAddedExchangeRates()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        await provider.GetExchangeRatesAsync(currencies);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Adding exchange rate")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_AllRatesHaveSameDate()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();

        // Act
        var result = await provider.GetExchangeRatesAsync(_multipleRequestedCurrencies);

        // Assert
        var rates = result.ToList();
        var distinctDates = rates.Select(r => r.Date).Distinct().ToList();
        Assert.Single(distinctDates);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_AllTargetCurrenciesAreCZK()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();

        // Act
        var result = await provider.GetExchangeRatesAsync(_multipleRequestedCurrencies);

        // Assert
        var rates = result.ToList();
        Assert.All(rates, rate => Assert.Equal("CZK", rate.TargetCurrency.Code));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithHttpError_ThrowsHttpRequestException()
    {
        // Arrange
        SetupHttpResponse("Internal Server Error", HttpStatusCode.InternalServerError);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidJson_ThrowsJsonException()
    {
        // Arrange
        SetupHttpResponse("INVALID JSON");
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_CallsCorrectUrlWithCurrentDate()
    {
        // Arrange
        SetupHttpResponse(_validJsonResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };
        var expectedDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Act
        await provider.GetExchangeRatesAsync(currencies);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get &&
                req.RequestUri.ToString().Contains($"date={expectedDate}") &&
                req.RequestUri.ToString().Contains("lang=EN")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidDateFormat_ThrowsFormatException()
    {
        // Arrange
        var responseWithInvalidDate = new ExchangeRateResponseDto
        {
            Rates = new List<RateDto>
            {
                new() { CurrencyCode = "USD", Amount = 1, Rate = 22.500m, ValidFor = "INVALID" }
            }
        };
        SetupHttpResponse(JsonSerializer.Serialize(responseWithInvalidDate));
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithDecimalRateValue_ParsesCorrectly()
    {
        // Arrange
        var responseWithDecimal = new ExchangeRateResponseDto
        {
            Rates = new List<RateDto>
            {
                new() { CurrencyCode = "USD", Amount = 1, Rate = 22.567m, ValidFor = "2025-10-01" }
            }
        };
        SetupHttpResponse(JsonSerializer.Serialize(responseWithDecimal));
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Equal(22.567m, rates[0].Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithNullDto_ReturnsEmptyList()
    {
        // Arrange
        SetupHttpResponse("null");
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }
}