using System.Net;
using System.Net.Http.Json;
using ExchangeRateService.Contracts;
using ExchangeRateService.Domain;
using ExchangeRateService.ExternalServices;
using ExchangeRateService.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExchangeRateService.IntegrationTests;

public class ExchangeRateEndpointsTests(TestWebApplicationFactory<Program> factory)
    : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly HttpClient _httpClient = CreateClientWithReplaced(factory, new Mock<ICNBClientService>());

    public static IEnumerable<object[]> InvalidCurrencyCodes => new List<object[]>
    {
        new object[] { "currencyCode=abba", "Only three-letter ISO 4217 code of the currency is supported." },
        new object[] { "currencyCode=USD&currencyCode=310&currencyCode=CZK", "Only three-letter ISO 4217 code of the currency is supported." }
    };

    [Fact]
    public async Task GetExchangeRates_WithoutFilter_ReturnsAllAvailableCurrencies()
    {
        var clientServiceMock = new Mock<ICNBClientService>();
        clientServiceMock.Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ExchangeRate[]>(new ExchangeRate[] 
            {
                new() { SourceCurrency = new Currency("JPY"), TargetCurrency = new Currency("CZK"), Value = 0.62631f },
            }));
        var client = CreateClientWithReplaced<ICNBClientService>(_factory, clientServiceMock);
        var response = await client.GetAsync("/api/exchangeRate");
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
    
    [Fact]
    public async Task GetExchangeRates_ForDefaultTaskCurrencies_ReturnsFilteredExchangeRates()
    {
        // Arrange
        var clientServiceMock = new Mock<ICNBClientService>();
        clientServiceMock.Setup(service => service.GetDailyExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ExchangeRate[]>(new ExchangeRate[] 
                {
                    new() { SourceCurrency = new Currency("JPY"), TargetCurrency = new Currency("CZK"), Value = 0.62631f },
                    new() { SourceCurrency = new Currency("AAL"), TargetCurrency = new Currency("CZK"), Value = 62.624f },
                    new() { SourceCurrency = new Currency("THB"), TargetCurrency = new Currency("CZK"), Value = 1.262f },
                    new() { SourceCurrency = new Currency("USD"), TargetCurrency = new Currency("CZK"), Value = 27.624f },
                }));
        var client = CreateClientWithReplaced<ICNBClientService>(_factory, clientServiceMock);
        var taskDefaultCurrencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
        var filterQuery = "currencyCode=" + string.Join("&currencyCode=", taskDefaultCurrencies);
        var expectedResult = new List<string> { "JPY/CZK=0,62631", "THB/CZK=1,262", "USD/CZK=27,624" };
        
        // Act
        var exchangeRates = await client.GetFromJsonAsync<ExchangeRateResponse>("/api/exchangeRate?" + filterQuery);

        // Assert
        Assert.NotNull(exchangeRates);
        Assert.NotEmpty(exchangeRates);
        // Better with FluentAssertion package
        Assert.Equivalent(expectedResult, exchangeRates);
    }

    private static HttpClient CreateClientWithReplaced<TType>(TestWebApplicationFactory<Program> factory, Mock<TType> mock) where TType : class
        => factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TType));
                    if (descriptor != null) 
                    {
                        services.Remove(descriptor);
                    }
                    services.AddSingleton<TType>(_ => mock.Object);
                });
            })
            .CreateClient();
    
}