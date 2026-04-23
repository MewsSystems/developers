using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdaterModels.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using Xunit;

public class CNBExchangeRetreivalControllerTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly CNBExchangeRetreivalController _controller;

    public CNBExchangeRetreivalControllerTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.cnb.cz")
        };
        _controller = new CNBExchangeRetreivalController(_httpClient);
    }

    [Fact]
    public async Task GetRatesAsync_ReturnsExchangeRates()
    {
        // Arrange
        var jsonResponse = new JObject(
            new JProperty("rates",
                new JArray(
                    new JObject(
                        new JProperty("currencyCode", "USD"),
                        new JProperty("rate", "23.50")
                    ),
                    new JObject(
                        new JProperty("currencyCode", "EUR"),
                        new JProperty("rate", "25.00")
                    )
                )
            )
        ).ToString();

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        // Act
        var result = await _controller.GetRatesAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var exchangeRates = Assert.IsType<List<ExchangeRateModel>>(okResult.Value);
        Assert.Equal(2, exchangeRates.Count);
        Assert.Equal("USD", exchangeRates[0].SourceCurrency.Code);
        Assert.Equal(23.50m, exchangeRates[0].Value);
        Assert.Equal("EUR", exchangeRates[1].SourceCurrency.Code);
        Assert.Equal(25.00m, exchangeRates[1].Value);
    }
}
