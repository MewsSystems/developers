namespace ExchangeRateProvider.Host.WebApi.Tests.Controllers;

using System.Net;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Host.WebApi.Controllers;
using Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

[TestFixture]
public class ExchangeRateControllerTests
{
    [SetUp]
    public void Setup()
    {
        _exchangeRateServiceMock = new Mock<IExchangeRateService>();
        _controller = new ExchangeRateController(_exchangeRateServiceMock.Object);
    }

    private Mock<IExchangeRateService> _exchangeRateServiceMock;
    private ExchangeRateController _controller;

    [Test]
    public async Task GetExchangeRates_ShouldReturnOk_WhenValidCurrenciesProvided()
    {
        // Arrange
        var request = new GetExchangeRatesRequest { Items = new List<string> { "USD", "EUR" } };
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5),
            new(new Currency("EUR"), new Currency("CZK"), 25.3)
        };

        var expectedResult = new ExchangeRateResult(exchangeRates, new List<string>());
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRatesAsync(request.Items))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetExchangeRates(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var response = okResult.Value as GetExchangeRatesResponse;
        response.Should().NotBeNull();
        response.Rates.Should().HaveCount(2);
        response.CurrenciesNotResolved.Should().BeEmpty();
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnBadRequest_WhenRequestIsNull()
    {
        // Act
        var result = await _controller.GetExchangeRates(null!);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = result as BadRequestObjectResult;
        badRequest!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        badRequest.Value.Should().Be("Currencies list cannot be empty.");
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnBadRequest_WhenCurrenciesListIsEmpty()
    {
        // Arrange
        var request = new GetExchangeRatesRequest { Items = new List<string>() };

        // Act
        var result = await _controller.GetExchangeRates(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = result as BadRequestObjectResult;
        badRequest!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        badRequest.Value.Should().Be("Currencies list cannot be empty.");
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnOk_WithUnresolvedCurrencies()
    {
        // Arrange
        var request = new GetExchangeRatesRequest { Items = new List<string> { "USD", "EUR", "XYZ" } };
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5)
        };

        var expectedResult = new ExchangeRateResult(exchangeRates, new List<string> { "EUR", "XYZ" });
        _exchangeRateServiceMock.Setup(s => s.GetExchangeRatesAsync(request.Items))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetExchangeRates(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var response = okResult.Value as GetExchangeRatesResponse;
        response.Should().NotBeNull();
        response.Rates.Should().HaveCount(1);
        response.CurrenciesNotResolved.Should().Contain([ "EUR", "XYZ" ]);
    }
}
