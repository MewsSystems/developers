using ExchangeRateApi.Controllers;
using ExchangeRateApi.Models;
using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
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
    private ExchangeRateController _controller = null!;

    private static readonly CancellationToken TestToken = CancellationToken.None;

    [SetUp]
    public void SetUp()
    {
        _exchangeRateService = Substitute.For<IExchangeRateService>();
        _logger = Substitute.For<ILogger<ExchangeRateController>>();
        _controller = new ExchangeRateController(_exchangeRateService, _logger);
    }

    [Test]
    public async Task GetExchangeRates_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD","EUR"}, TargetCurrency = "CZK" };
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5m),
            new(new Currency("EUR"), new Currency("CZK"), 24.0m)
        };
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
    public async Task GetExchangeRates_LowercaseCode_ReturnsBadRequest()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"usd"}, TargetCurrency = "CZK" }; // invalid casing

        // Act
        var result = await _controller.GetExchangeRates(request, TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var bad = (BadRequestObjectResult)result.Result!;
            Assert.That(bad.Value, Is.InstanceOf<ErrorResponse>());
            Assert.That(((ErrorResponse)bad.Value!).Error, Does.Contain("uppercase"));
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Warning, "Validation failed for request");
        });
    }

    [Test]
    public void GetExchangeRates_NoCodes_ThrowsArgumentException()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>(), TargetCurrency = "CZK" };

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _controller.GetExchangeRates(request, TestToken));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Does.Contain("At least one currency code"));
            _logger.VerifyLogContaining<ExchangeRateController>(1, LogLevel.Warning, "empty currency codes");
        });
    }

    [Test]
    public async Task GetExchangeRates_DefaultTargetCurrency_WhenNull()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = null };
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
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
    public void GetExchangeRates_InvalidOperation_Propagates()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = "XXX" };
        _exchangeRateService.When(s => s.GetExchangeRatesAsync("XXX", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()))
            .Do(_ => throw new InvalidOperationException("no provider"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.GetExchangeRates(request, TestToken));
    }

    [Test]
    public void GetExchangeRates_UnexpectedException_BubblesUp()
    {
        // Arrange
        var request = new ExchangeRateRequest { CurrencyCodes = new List<string>{"USD"}, TargetCurrency = "CZK" };
        _exchangeRateService.When(s => s.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()))
            .Do(_ => throw new Exception("boom"));

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _controller.GetExchangeRates(request, TestToken));
        Assert.That(ex!.Message, Is.EqualTo("boom"));
    }

    // GET endpoint tests
    [Test]
    public async Task GetExchangeRatesQuery_Valid_ReturnsOk()
    {
        // Arrange
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);

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
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var bad = (BadRequestObjectResult)result.Result!;
            Assert.That(bad.Value, Is.InstanceOf<ErrorResponse>());
            Assert.That(((ErrorResponse)bad.Value!).Error, Does.Contain("format"));
        });
    }

    [Test]
    public async Task GetExchangeRatesQuery_TooLong_ReturnsBadRequest()
    {
        // Arrange
        var longString = new string('A', 1001);
        // Act
        var result = await _controller.GetExchangeRatesQuery(longString, "CZK", TestToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var bad = (BadRequestObjectResult)result.Result!;
            Assert.That(((ErrorResponse)bad.Value!).Error, Does.Contain("too long"));
        });
    }

    [Test]
    public async Task GetExchangeRatesQuery_ManyCodes_ReturnsOk()
    {
        // Arrange - validate controller accepts more than 10 (limit removed in refactor)
        var codes = string.Join(',', new [] { "USD","EUR","JPY","GBP","CHF","AUD","CAD","NZD","CNY","INR","PLN" });
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);

        // Act
        var result = await _controller.GetExchangeRatesQuery(codes, "CZK", TestToken);
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetExchangeRatesQuery_DefaultTarget_WhenNull()
    {
        // Arrange
        var rates = new List<ExchangeRate>{ new(new Currency("USD"), new Currency("CZK"), 22.5m)};
        _exchangeRateService.GetExchangeRatesAsync("CZK", Arg.Any<IEnumerable<Currency>>(), Arg.Any<CancellationToken>()).Returns(rates);

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
