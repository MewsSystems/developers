using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ExchangeRateUpdater.Api.Models;
using NSubstitute;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Api.Controllers;

namespace ExchangeRateUpdater.Tests.Api;

public class ExchangeRatesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRatesController> _logger;
    private readonly ExchangeRatesController _sut;

    public ExchangeRatesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _exchangeRateService = Substitute.For<IExchangeRateService>();
        _logger = Substitute.For<ILogger<ExchangeRatesController>>();

        _sut = new ExchangeRatesController(_exchangeRateService, _logger);
    }

    [Fact]
    public async Task GetExchangeRates_ValidCurrencies_ShouldReturnOk()
    {
        // Act
        var response = await _sut.GetExchangeRates("USD,EUR");

        // Assert
        var result = response.Result as OkObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, result?.StatusCode);

        var apiResponse = result?.Value as ApiResponse<ExchangeRateResponse>;

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEmpty(apiResponse.Data.Rates);
    }

    [Fact]
    public async Task GetExchangeRates_EmptyCurrencies_ShouldReturnBadRequest()
    {
        // Act
        var response = await _sut.GetExchangeRates("");

        // Assert
        var result = response.Result as BadRequestObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, result?.StatusCode);
    }

    [Fact]
    public async Task GetExchangeRates_NoCurrenciesParameter_ShouldReturnBadRequest()
    {
        // Act
        var response = await _sut.GetExchangeRates("");

        // Assert
        var result = response.Result as BadRequestObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, result?.StatusCode);
    }
}
