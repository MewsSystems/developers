using ExchangeRateUpdater.Business;
using ExchangeRateUpdater.Controllers;
using ExchangeRateUpdater.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdaterTests.Controllers;

public class ExchangeRatesControllerTests
{
    private readonly Mock<IExchangeRateProvider> _exchangeRateProviderMock;
    private readonly Mock<IValidator<string[]>> _currencyCodeValidatorMock;
    private readonly Mock<ILogger<ExchangeRatesController>> _loggerMock;
    private readonly ExchangeRatesController _sut;

    public ExchangeRatesControllerTests()
    {
        _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
        _currencyCodeValidatorMock = new Mock<IValidator<string[]>>();
        _loggerMock = new Mock<ILogger<ExchangeRatesController>>();
        _sut = new ExchangeRatesController(
            _exchangeRateProviderMock.Object,
            _currencyCodeValidatorMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Given_InvalidCurrencies_When_GetSelectedCurrencyCodesDaily_Then_ReturnsBadRequest()
    {
        var currencies = new[] { "INVALID" };
        var validationFailures = new List<ValidationFailure> { new("currencies", "Invalid currency code") };
        _currencyCodeValidatorMock
            .Setup(v => v.ValidateAsync(currencies, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var result = await _sut.GetSelectedCurrencyCodesDaily(currencies);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new[] { "Invalid currency code" });

        _loggerMock.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString()!.Contains("ExchangeRatesController_ValidationFailure") && v.ToString()!.Contains(validationFailures[0].ErrorMessage)),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task Given_ProviderReturnsFailure_When_GetSelectedCurrencyCodesDaily_Then_ReturnsBadRequestWithError()
    {
        const string errorMessage = "Provider error";
        var currencies = new[] { "USD" };
        _currencyCodeValidatorMock
            .Setup(validator => validator.ValidateAsync(currencies, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _exchangeRateProviderMock
            .Setup(provider => provider.GetExchangeRates(currencies))
            .ReturnsAsync(Result<IEnumerable<ExchangeRate>>.Fail(errorMessage));

        var result = await _sut.GetSelectedCurrencyCodesDaily(currencies);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(errorMessage);
        
        _loggerMock.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString()!.Contains("ExchangeRatesController_ProviderError") && 
                v.ToString()!.Contains(errorMessage)),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task Given_ValidCurrencyCodes_When_GetSelectedCurrencyCodesDaily_Then_ReturnsOkWithRates()
    {
        var currencyCodes = new[] { "USD" };
        var rates = new List<ExchangeRate> { new(new Currency("CZK"), new Currency("USD"), 25.0m) };
        _currencyCodeValidatorMock
            .Setup(validator => validator.ValidateAsync(currencyCodes, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _exchangeRateProviderMock
            .Setup(provider => provider.GetExchangeRates(currencyCodes))
            .ReturnsAsync(Result<IEnumerable<ExchangeRate>>.Ok(rates));

        var result = await _sut.GetSelectedCurrencyCodesDaily(currencyCodes);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(rates);
    }
}
