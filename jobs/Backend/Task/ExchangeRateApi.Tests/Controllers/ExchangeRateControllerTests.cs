using ExchangeRateApi.Controllers;
using ExchangeRateApi.Models;
using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ExchangeRateApi.Tests.TestHelpers;

namespace ExchangeRateApi.Tests.Controllers;

[TestFixture]
public class ExchangeRateControllerTests
{
    private IExchangeRateService _exchangeRateService = null!;
    private ILogger<ExchangeRateController> _logger = null!;
    private IValidator<ExchangeRateRequest> _validator = null!;
    private ExchangeRateController _controller = null!;

    private static readonly CancellationToken TestToken = CancellationToken.None;

    [SetUp]
    public void SetUp()
    {
        _exchangeRateService = Substitute.For<IExchangeRateService>();
        _logger = Substitute.For<ILogger<ExchangeRateController>>();
        _validator = Substitute.For<IValidator<ExchangeRateRequest>>();
        _controller = new ExchangeRateController(_exchangeRateService, _logger, _validator);
    }

    [Test]
    public async Task GetExchangeRates_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"usd","eur"}, TargetCurrency = "CZK" };
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5m),
            new(new Currency("EUR"), new Currency("CZK"), 24.0m)
        };
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var response = (ExchangeRateResponse)((OkObjectResult)result.Result!).Value!;
            Assert.That(response.Rates, Has.Count.EqualTo(2));
            Assert.That(response.TargetCurrency, Is.EqualTo("CZK"));
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Successfully retrieved");
        });
    }

    [Test]
    public async Task GetExchangeRates_ValidatorErrors_ReturnsBadRequest()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = "CZK" };
        var failures = new List<FluentValidation.Results.ValidationFailure>{ new("CurrencyCodes","Invalid") };
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(failures));

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var bad = (BadRequestObjectResult)result.Result!;
            Assert.That(bad.Value, Is.InstanceOf<string[]>());
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Warning, "Validation failed for request");
        });
    }

    [Test]
    public async Task GetExchangeRates_NoCodes_ReturnsBadRequest()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>(), TargetCurrency = "CZK" };

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Warning, "empty currency codes");
        });
    }

    [Test]
    public async Task GetExchangeRates_DefaultTargetCurrency_WhenNull()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = null };
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            var response = (ExchangeRateResponse)((OkObjectResult)result.Result!).Value!;
            Assert.That(response.TargetCurrency, Is.EqualTo("CZK"));
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Successfully retrieved");
        });
    }

    [Test]
    public async Task GetExchangeRates_InvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = "XXX" };
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _exchangeRateService.When(s => s.GetExchangeRatesAsync("XXX", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()))
            .Do(_ => throw new InvalidOperationException("no provider"));

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Error, "Invalid operation when getting exchange rates");
        });
    }

    [Test]
    public async Task GetExchangeRates_Exception_Returns500()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = "CZK" };
        _validator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _exchangeRateService.When(s => s.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()))
            .Do(_ => throw new Exception("boom"));

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<ObjectResult>());
            var obj = (ObjectResult)result.Result!;
            Assert.That(obj.StatusCode, Is.EqualTo(500));
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Error, "Unexpected error occurred while getting exchange rates");
        });
    }

    // GET endpoint tests
    [Test]
    public async Task GetExchangeRatesQuery_Valid_ReturnsOk()
    {
        // Arrange
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);
        _validator.ValidateAsync(Arg.Any<ExchangeRateRequest>(), Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _controller.GetExchangeRatesQuery("USD", "CZK", TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Successfully retrieved");
        });
    }

    [Test]
    public async Task GetExchangeRatesQuery_InvalidFormat_ReturnsBadRequest()
    {
        // Arrange / Act
        var result = await _controller.GetExchangeRatesQuery("USDX,EUR", "CZK", TestToken);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetExchangeRatesQuery_TooManyCodes_ReturnsBadRequest()
    {
        // Arrange
        var codes = string.Join(',', Enumerable.Range(0,11).Select(i => $"A{i:00}".Substring(0,3)));

        // Act
        var result = await _controller.GetExchangeRatesQuery("USD,EUR,JPY,AAA,BBB,CCC,DDD,EEE,FFF,GGG,HHH", "CZK", TestToken);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetExchangeRatesQuery_DefaultTarget_WhenNull()
    {
        // Arrange
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);
        _validator.ValidateAsync(Arg.Any<ExchangeRateRequest>(), Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _controller.GetExchangeRatesQuery("USD", null, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            var ok = (OkObjectResult)result.Result!;
            var response = (ExchangeRateResponse)ok.Value!;
            Assert.That(response.TargetCurrency, Is.EqualTo("CZK"));
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Received request for exchange rates");
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Information, "Successfully retrieved");
        });
    }

    [Test]
    public void GetAvailableProviders_ReturnsOkWithProviderList()
    {
        // Arrange / Act
        var result = _controller.GetAvailableProviders();

        // Assert
        Assert.Multiple(() =>
        {
            var ok = result.Result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
        });
    }
}
