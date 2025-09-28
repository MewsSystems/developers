using Microsoft.AspNetCore.Mvc;
using ExchangeRateUpdater.Api.Controllers;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests.Api;

public class ExchangeRatesControllerTests
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRatesController> _logger;
    private readonly ExchangeRatesController _sut;

    public ExchangeRatesControllerTests()
    {
        _exchangeRateService = Substitute.For<IExchangeRateService>();
        _logger = Substitute.For<ILogger<ExchangeRatesController>>();
        _sut = new ExchangeRatesController(_exchangeRateService, _logger);
    }

    [Fact]
    public async Task GetExchangeRates_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 21.5m, DateHelper.Today),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 25.5m, DateHelper.Today)
        };

        _exchangeRateService
            .GetExchangeRates(
                Arg.Is<IEnumerable<Currency>>(c => c.Any(x => x.Code == "USD" || x.Code == "EUR")), 
                Arg.Any<Maybe<DateOnly>>())
            .Returns(expectedRates);

        // Act
        var result = await _sut.GetExchangeRates(["USD", "EUR"]);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<ApiResponse<ExchangeRateResponseDto>>()
            .Which.Should().Match<ApiResponse<ExchangeRateResponseDto>>(r => 
                r.Success &&
                r.Data != null &&
                r.Data.Rates.Count() == 2);
    }

    [Fact]
    public async Task GetExchangeRates_ServiceThrowsException_PropagatesException()
    {
        // Arrange
        _exchangeRateService
            .GetExchangeRates(Arg.Any<IEnumerable<Currency>>(), Arg.Any<Maybe<DateOnly>>())
            .Returns(Task.FromException<IEnumerable<ExchangeRate>>(
                new ExchangeRateProviderException("Provider error")));

        // Act & Assert
        var action = () => _sut.GetExchangeRates(["USD", "EUR"]);

        await action.Should().ThrowAsync<ExchangeRateProviderException>()
            .WithMessage("Provider error");
    }

    [Fact]
    public async Task GetExchangeRates_NoRatesFound_ReturnsNotFound()
    {
        // Arrange
        _exchangeRateService
            .GetExchangeRates(Arg.Any<IEnumerable<Currency>>(), Arg.Any<Maybe<DateOnly>>())
            .Returns(Array.Empty<ExchangeRate>());

        // Act
        var result = await _sut.GetExchangeRates(["USD", "EUR"]);

        // Assert
        var response = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var apiResponse = response.Value.Should().BeOfType<ApiResponse>().Subject;
        
        apiResponse.Success.Should().BeFalse();
        apiResponse.Errors.Should().ContainMatch("*No exchange rates found*");
    }
}
