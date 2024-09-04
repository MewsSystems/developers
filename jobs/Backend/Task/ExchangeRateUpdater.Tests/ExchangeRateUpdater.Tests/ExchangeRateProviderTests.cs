using System.Net;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private readonly HttpClient _httpClient;
    private static readonly MockHttpMessageHandler _mockHttpMessageHandler = new();
    
    private readonly ExchangeRateProvider _exchangeRateProvider;
    
    public ExchangeRateProviderTests()
    {
        _httpClient = new HttpClient(_mockHttpMessageHandler);
        
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient, "https://example.com");
    }
    
    [Fact(DisplayName = "GetExchangeRates with 0 results from API")]
    public async Task GetExchangeRates0Results()
    {
        // Arrange
        _mockHttpMessageHandler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
                                        {
                                            "rates": []
                                        }
                                        """)
        };

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(TestData.Currencies);

        // Assert
        Assert.Empty(result);
    }
}