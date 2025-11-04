using System.Globalization;
using System.Net;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.RateProviders;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests.Services.RateProviders;

[Trait("Category", "Unit")]
public class CzechNationalBankCsvExchangeRateProviderTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly Mock<IAppConfiguration> _mockConfig;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<ILogger<CzechNationalBankCsvExchangeRateProvider>> _mockLogger;
    private readonly List<Currency> _multipleRequestedCurrencies;
    private readonly string _testUrl;
    private readonly Currency _usdCurrency;
    private readonly string _validCsvResponse;

    public CzechNationalBankCsvExchangeRateProviderTests()
    {
        _mockLogger = new Mock<ILogger<CzechNationalBankCsvExchangeRateProvider>>();
        _mockConfig = new Mock<IAppConfiguration>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        _testUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        _mockConfig.Setup(c => c.DailyRateUrl).Returns(_testUrl);
        _mockConfig.Setup(c => c.CzkCurrencyCode).Returns("CZK");

        _validCsvResponse = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.234
Brazil|real|1|BRL|4.123
EMU|euro|1|EUR|25.000
United Kingdom|pound|1|GBP|30.000
USA|dollar|1|USD|22.500";

        _usdCurrency = new Currency("USD");
        var eurCurrency = new Currency("EUR");
        var gbpCurrency = new Currency("GBP");

        _multipleRequestedCurrencies = new List<Currency> { _usdCurrency, eurCurrency, gbpCurrency };
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    private CzechNationalBankCsvExchangeRateProvider CreateProvider()
    {
        return new CzechNationalBankCsvExchangeRateProvider(_mockLogger.Object, _mockConfig.Object, _httpClient);
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
        SetupHttpResponse(_validCsvResponse);
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
        SetupHttpResponse(_validCsvResponse);
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
        SetupHttpResponse(_validCsvResponse);
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
        SetupHttpResponse(_validCsvResponse);
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
        var csvWithAmount = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
Japan|yen|100|JPY|15.000";
        SetupHttpResponse(csvWithAmount);
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
        SetupHttpResponse(_validCsvResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        var expectedDate = DateOnly.ParseExact("01 Oct 2025", "dd MMM yyyy", CultureInfo.InvariantCulture);
        Assert.Equal(expectedDate, rates[0].Date);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithMalformedLine_SkipsInvalidLine()
    {
        // Arrange
        var csvWithMalformedLine = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|22.500
INVALID LINE
United Kingdom|pound|1|GBP|30.000";
        SetupHttpResponse(csvWithMalformedLine);
        var provider = CreateProvider();

        // Act
        var result = await provider.GetExchangeRatesAsync(_multipleRequestedCurrencies);

        // Assert
        var rates = result.ToList();
        Assert.Equal(2, rates.Count);
        Assert.Contains(rates, r => r.SourceCurrency.Code == "USD");
        Assert.Contains(rates, r => r.SourceCurrency.Code == "GBP");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithLineHavingLessThanFiveParts_SkipsLine()
    {
        // Arrange
        var csvWithIncompleteLine = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|22.500
Country|Currency|Amount|Code";
        SetupHttpResponse(csvWithIncompleteLine);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Single(rates);
        Assert.Equal("USD", rates[0].SourceCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithCaseInsensitiveCurrencyCode_FindsMatch()
    {
        // Arrange
        SetupHttpResponse(_validCsvResponse);
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
    public async Task GetExchangeRatesAsync_WithEmptyResponse_ThrowsIndexOutOfRangeException()
    {
        // Arrange
        SetupHttpResponse(string.Empty);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<IndexOutOfRangeException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithOnlyHeaders_ReturnsEmptyList()
    {
        // Arrange
        var csvWithOnlyHeaders = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate";
        SetupHttpResponse(csvWithOnlyHeaders);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithDecimalValueHavingComma_ParsesCorrectly()
    {
        // Arrange
        var csvWithDecimal = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|22.567";
        SetupHttpResponse(csvWithDecimal);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Equal(22.567m, rates[0].Value);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithWhitespaceInLines_TrimsCorrectly()
    {
        // Arrange
        var csvWithWhitespace = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
  USA|dollar|1|USD|22.500  ";
        SetupHttpResponse(csvWithWhitespace);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        var result = await provider.GetExchangeRatesAsync(currencies);

        // Assert
        var rates = result.ToList();
        Assert.Single(rates);
        Assert.Equal("USD", rates[0].SourceCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_LogsDebugMessages()
    {
        // Arrange
        SetupHttpResponse(_validCsvResponse);
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
    public async Task GetExchangeRatesAsync_AllRatesHaveSameDate()
    {
        // Arrange
        SetupHttpResponse(_validCsvResponse);
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
        SetupHttpResponse(_validCsvResponse);
        var provider = CreateProvider();

        // Act
        var result = await provider.GetExchangeRatesAsync(_multipleRequestedCurrencies);

        // Assert
        var rates = result.ToList();
        Assert.All(rates, rate => Assert.Equal("CZK", rate.TargetCurrency.Code));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidAmountFormat_ThrowsFormatException()
    {
        // Arrange
        var csvWithInvalidAmount = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
USA|dollar|INVALID|USD|22.500";
        SetupHttpResponse(csvWithInvalidAmount);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidRateFormat_ThrowsFormatException()
    {
        // Arrange
        var csvWithInvalidRate = @"01 Oct 2025 #191
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|INVALID";
        SetupHttpResponse(csvWithInvalidRate);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() =>
            provider.GetExchangeRatesAsync(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithInvalidDateFormat_ThrowsFormatException()
    {
        // Arrange
        var csvWithInvalidDate = @"INVALID DATE #191
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|22.500";
        SetupHttpResponse(csvWithInvalidDate);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() =>
            provider.GetExchangeRatesAsync(currencies));
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
    public async Task GetExchangeRatesAsync_CallsConfiguredUrl()
    {
        // Arrange
        SetupHttpResponse(_validCsvResponse);
        var provider = CreateProvider();
        var currencies = new List<Currency> { _usdCurrency };

        // Act
        await provider.GetExchangeRatesAsync(currencies);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get &&
                req.RequestUri.ToString() == _testUrl),
            ItExpr.IsAny<CancellationToken>());
    }
}