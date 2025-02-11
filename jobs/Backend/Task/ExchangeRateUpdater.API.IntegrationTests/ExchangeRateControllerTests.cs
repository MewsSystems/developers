using ExchangeRateUpdater.Client;
using System.Text.Json;
using Xunit;

namespace ExchangeRateUpdater.API.IntegrationTests;

public class ExchangeRateControllerTests
{
    private readonly ExchangeRateApiClient _apiClient;

    public ExchangeRateControllerTests()
    {
        var apiBaseUrl = GetApiBaseUrl();
        _apiClient = new ExchangeRateApiClient(apiBaseUrl, new HttpClient());
    }

    [Fact]
    public async Task GetExchangeRates_Returns200_WithValidDate()
    {
        // Arrange
        var validDate = new DateTimeOffset(2024, 02, 10, 0, 0, 0, TimeSpan.Zero);
        var currencies = new[] { "USD", "EUR" };

        // Act
        var result = await _apiClient.ExchangeRateAsync(validDate, currencies);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.ExchangeRates);
        Assert.Contains(result.ExchangeRates, r => r.TargetCurrency.Code == "USD");
    }

    [Fact]
    public async Task GetExchangeRates_Returns412_WithFutureDate()
    {
        // Arrange
        var futureDate = DateTimeOffset.UtcNow.AddDays(1);
        var currencies = new[] { "USD" };

        // Act
        var ex = await Assert.ThrowsAsync<ApiException>(() =>
            _apiClient.ExchangeRateAsync(futureDate, currencies));

        // Assert
        Assert.Equal(412, ex.StatusCode);

        // Deserialize error response
        var errorResponse = DeserializeErrorResponse(ex.Response);
        Assert.Equal("Validation Failure", errorResponse.Title);
        Assert.Contains("Date", errorResponse.Errors.Keys);
    }

    [Fact]
    public async Task GetExchangeRates_Returns412_IfDateNotSupported()
    {
        // Arrange
        var missingDate = new DateTimeOffset(1988, 01, 01, 0, 0, 0, TimeSpan.Zero);
        var currencies = new[] { "USD" };

        // Act
        var ex = await Assert.ThrowsAsync<ApiException>(() =>
            _apiClient.ExchangeRateAsync(missingDate, currencies));

        /// Assert
        Assert.Equal(412, ex.StatusCode);

        // Deserialize error response
        var errorResponse = DeserializeErrorResponse(ex.Response);
        Assert.Equal("Validation Failure", errorResponse.Title);
        Assert.Contains("Date", errorResponse.Errors.Keys);
    }

    [Fact]
    public async Task GetExchangeRates_Returns412_WithInvalidCurrencyCode()
    {
        // Arrange
        var validDate = new DateTimeOffset(2024, 02, 10, 0, 0, 0, TimeSpan.Zero);
        var invalidCurrencies = new[] { "US", "EURO" }; // Incorrect formats

        // Act
        var ex = await Assert.ThrowsAsync<ApiException>(() =>
            _apiClient.ExchangeRateAsync(validDate, invalidCurrencies));

        // Assert
        Assert.Equal(412, ex.StatusCode);

        // Deserialize error response
        var errorResponse = DeserializeErrorResponse(ex.Response);
        Assert.Equal("Validation Failure", errorResponse.Title);
        Assert.Contains("Currencies", errorResponse.Errors.Keys);
    }



    [Fact]
    public async Task GetExchangeRates_Returns412_WithTooManyCurrencies()
    {
        // Arrange
        var validDate = new DateTimeOffset(2024, 02, 10, 0, 0, 0, TimeSpan.Zero);
        var tooManyCurrencies = new[] { "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP", "USD", "EUR", "GBP" };

        // Act
        var ex = await Assert.ThrowsAsync<ApiException>(() =>
            _apiClient.ExchangeRateAsync(validDate, tooManyCurrencies));

        // Assert
        Assert.Equal(412, ex.StatusCode);

        // Deserialize error response
        var errorResponse = DeserializeErrorResponse(ex.Response);
        Assert.Equal("Validation Failure", errorResponse.Title);
        Assert.Contains("Currencies", errorResponse.Errors.Keys);
    }

    private static string GetApiBaseUrl()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Configuration file not found.", configPath);
        }

        var configContent = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ApiTestConfig>(configContent);
        return config?.ApiBaseUrl ?? throw new InvalidOperationException("API Base URL is missing.");
    }

    private static ApiErrorResponse DeserializeErrorResponse(string responseJson)
    {
        return JsonSerializer.Deserialize<ApiErrorResponse>(
            responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Failed to deserialize API error response.");
    }

    private class ApiTestConfig
    {
        public string ApiBaseUrl { get; set; }
    }
}
