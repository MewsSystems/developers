using System.Net;
using System.Net.Http.Json;
using ExchangeRateService.Contracts;
using ExchangeRateService.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;

namespace ExchangeRateService.IntegrationTests;

public class ExchangeRateEndpointsTests(TestWebApplicationFactory<Program> factory)
    : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly HttpClient _httpClient = factory.CreateClient();

    public static IEnumerable<object[]> InvalidCurrencyCodes => new List<object[]>
    {
        new object[] { "currencyCode=abba", "Only three-letter ISO 4217 code of the currency is supported." },
        new object[] { "currencyCode=USD&currencyCode=310&currencyCode=CZK", "Only three-letter ISO 4217 code of the currency is supported." }
    };

    [Fact]
    public async Task GetExchangeRates_WithoutFilter_ReturnsAllAvailableCurrencies()
    {
        var response = await _httpClient.GetAsync("/api/exchangeRate");
        var exchangeRates = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(exchangeRates);
        Assert.NotEmpty(exchangeRates);
    }
    
    [Theory]
    [MemberData(nameof(InvalidCurrencyCodes))]
    public async Task GetExchangeRates_WithInvalidCodes_ReturnsValidationProblems(string filterQuery, string errorMessage)
    {
        var response = await _httpClient.GetAsync("/api/exchangeRate?" + filterQuery);
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemResult?.Errors);
        Assert.Collection(problemResult.Errors, (error) => Assert.Equal(errorMessage, error.Value.First()));
    }
    
    // Todo return filtered data
}