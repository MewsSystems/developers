using System.Net;
using ExchangeRateProviderService.Controllers;
using ExchangeRateProviderService.Services;
using ExchangeRateUpdaterModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

public class ExchangeRateProviderControllerTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly DataRetrievalClient _dataRetrievalClient;
    private readonly ExchangeRateProviderController _controller;

    public ExchangeRateProviderControllerTests(IOptions<AppSettings> appSettings)
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:7156/api/CNBExchangeRetreival")
        };
        _dataRetrievalClient = new DataRetrievalClient(_httpClient, appSettings);
        _controller = new ExchangeRateProviderController(_dataRetrievalClient);
    }

    [Fact]
    public async Task GetExchangeRates_ReturnsFilteredExchangeRates()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRateModel>
        {
            new ExchangeRateModel(new CurrencyModel("USD"), new CurrencyModel("CZK"), 23.50m),
            new ExchangeRateModel(new CurrencyModel("EUR"), new CurrencyModel("CZK"), 25.00m)
        };

        var jsonResponse = JsonConvert.SerializeObject(exchangeRates);

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

        var currencyCodes = new List<CurrencyModel> ();
        currencyCodes.Add(new CurrencyModel("USD"));

        // Act
        var result = await _controller.GetExchangeRates(currencyCodes);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var filteredRates = Assert.IsType<List<ExchangeRateModel>>(okResult.Value);
        Assert.Single(filteredRates);
        Assert.Equal("USD", filteredRates[0].SourceCurrency.Code);
        Assert.Equal(23.50m, filteredRates[0].Value);
    }
}
