using ExchangeRateProvider.Api.Controllers;
using ExchangeRateProvider.Api.Validators;
using ExchangeRateProvider.Application.Abstractions;
using ExchangeRateProvider.Application.DTOs;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.ValueObjects;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Shouldly;

namespace ExchangeRateProvider.Api.Tests.Unit.Controllers;

public class ExchangeRateControllerTests
{
    private readonly IQueryHandler<GetExchangeRatesQuery, IList<ExchangeRateDto>> _fakeHandler;
    private readonly FakeLogger<ExchangeRateController> _logger;
    private readonly ExchangeRateController _controller;
    private readonly QuoteCurrenciesValidator _validator;

    public ExchangeRateControllerTests()
    {
        _fakeHandler = A.Fake<IQueryHandler<GetExchangeRatesQuery, IList<ExchangeRateDto>>>();
        _logger = new FakeLogger<ExchangeRateController>();
        _validator = new QuoteCurrenciesValidator();
        _controller = new ExchangeRateController(_fakeHandler, _validator, _logger);
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithValidBaseCurrency_ReturnsOkWithRates(string baseCurrency)
    {
        // Arrange
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m),
            new(new Currency(baseCurrency), new Currency("GBP"), 0.73m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q => q.BaseCurrency.Code == baseCurrency),
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, null, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result;
        okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);
        okResult.Value.ShouldBe(expectedRates);
        
        var logEntry = _logger.Collector.GetSnapshot().Single();
        logEntry.Level.ShouldBe(LogLevel.Information);
        logEntry.Message.ShouldContain($"BaseCurrency: {baseCurrency}");
        logEntry.Message.ShouldContain("QuoteCurrencies: null");
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithQuoteCurrencies_PassesThemToHandlerAndReturnsResult(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EUR", "GBP" };
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m),
            new(new Currency(baseCurrency), new Currency("GBP"), 0.73m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q =>
                q.BaseCurrency.Code == baseCurrency &&
                q.QuoteCurrencies!.Count == 2 &&
                q.QuoteCurrencies.Any(c => c.Code == "EUR") &&
                q.QuoteCurrencies.Any(c => c.Code == "GBP")),
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result;
        okResult.Value.ShouldBe(expectedRates);

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q =>
                q.QuoteCurrencies!.Any(c => c.Code == "EUR") &&
                q.QuoteCurrencies!.Any(c => c.Code == "GBP")),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        
        var logEntry = _logger.Collector.GetSnapshot().Single();
        logEntry.Level.ShouldBe(LogLevel.Information);
        logEntry.Message.ShouldContain($"BaseCurrency: {baseCurrency}");
        logEntry.Message.ShouldContain("QuoteCurrencies: EUR, GBP");
    }

    [Fact]
    public async Task GetByCurrency_WithNullBaseCurrency_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetByCurrency(null, null, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result;
        badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);

        var problemDetails = badRequestResult.Value.ShouldBeOfType<ProblemDetails>();
        problemDetails.Title.ShouldBe("Invalid base currency");
        problemDetails.Detail.ShouldBe("Base currency is required.");
        problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);

        A.CallTo(() => _fakeHandler.HandleAsync(A<GetExchangeRatesQuery>._, A<CancellationToken>._))
            .MustNotHaveHappened();
        
        var logEntry = _logger.Collector.GetSnapshot().Single();
        logEntry.Level.ShouldBe(LogLevel.Information);
        logEntry.Message.ShouldContain("BaseCurrency: ");
    }

    [Fact]
    public async Task GetByCurrency_WithEmptyBaseCurrency_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetByCurrency("", null, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result;

        var problemDetails = badRequestResult.Value.ShouldBeOfType<ProblemDetails>();
        problemDetails.Title.ShouldBe("Invalid base currency");
        problemDetails.Detail.ShouldBe("Base currency is required.");

        A.CallTo(() => _fakeHandler.HandleAsync(A<GetExchangeRatesQuery>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task GetByCurrency_WithWhitespaceBaseCurrency_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetByCurrency("   ", null, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result;

        var problemDetails = badRequestResult.Value.ShouldBeOfType<ProblemDetails>();
        problemDetails.Title.ShouldBe("Invalid base currency");
        problemDetails.Detail.ShouldBe("Base currency is required.");

        A.CallTo(() => _fakeHandler.HandleAsync(A<GetExchangeRatesQuery>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task GetByCurrency_WithUnsupportedBaseCurrency_ReturnsBadRequest()
    {
        // Arrange
        var unsupportedCurrency = "XXX";

        // Act
        var result = await _controller.GetByCurrency(unsupportedCurrency, null, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result;

        var problemDetails = badRequestResult.Value.ShouldBeOfType<ProblemDetails>();
        problemDetails.Title.ShouldBe("Unsupported base currency");
        problemDetails.Detail!.ShouldContain($"The currency '{unsupportedCurrency}' is not supported");
        problemDetails.Detail!.ShouldContain("Supported currencies:");
        problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);

        A.CallTo(() => _fakeHandler.HandleAsync(A<GetExchangeRatesQuery>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithInvalidQuoteCurrency_ReturnsBadRequest(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string> { "INVALID" };

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result;

        var problemDetails = badRequestResult.Value.ShouldBeOfType<ProblemDetails>();
        problemDetails.Title.ShouldBe("Invalid quote currencies");
        problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);

        A.CallTo(() => _fakeHandler.HandleAsync(A<GetExchangeRatesQuery>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithQuoteCurrenciesContainingEmptyStrings_FiltersThemOut(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EUR", "", "  ", "GBP" };
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m),
            new(new Currency(baseCurrency), new Currency("GBP"), 0.73m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q =>
                q.BaseCurrency.Code == baseCurrency &&
                q.QuoteCurrencies!.Count == 2),
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q =>
                q.QuoteCurrencies!.Count == 2 &&
                q.QuoteCurrencies.All(c => !string.IsNullOrWhiteSpace(c.Code))),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithEmptyQuoteCurrenciesList_PassesNullToHandler(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string>();
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q => q.QuoteCurrencies == null),
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q => q.QuoteCurrencies == null),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithLowercaseQuoteCurrencies_ConvertsToUppercase(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string> { "eur", "gbp" };
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m),
            new(new Currency(baseCurrency), new Currency("GBP"), 0.73m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>._,
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q =>
                q.QuoteCurrencies!.Any(c => c.Code == "EUR") &&
                q.QuoteCurrencies!.Any(c => c.Code == "GBP")),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_ReturnsEmptyList_WhenNoRatesAvailable(string baseCurrency)
    {
        // Arrange
        var expectedRates = new List<ExchangeRateDto>();

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>._,
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, null, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result.Result;
        var rates = okResult.Value.ShouldBeOfType<List<ExchangeRateDto>>();
        rates.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithValidThreeLetterQuoteCurrencies_PassesValidation(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string> { "EUR", "GBP", "JPY", "AUD" };
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>._,
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q => q.QuoteCurrencies!.Count == 4),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task GetByCurrency_WithMixedCaseValidQuoteCurrencies_ConvertsToUppercaseAndPassesValidation(string baseCurrency)
    {
        // Arrange
        var quoteCurrencies = new List<string> { "eur", "GBp", "JpY" }; // Mixed case but valid
        var expectedRates = new List<ExchangeRateDto>
        {
            new(new Currency(baseCurrency), new Currency("EUR"), 0.85m)
        };

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>._,
            A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _controller.GetByCurrency(baseCurrency, quoteCurrencies, CancellationToken.None);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();

        A.CallTo(() => _fakeHandler.HandleAsync(
            A<GetExchangeRatesQuery>.That.Matches(q =>
                q.QuoteCurrencies!.Count == 3 &&
                q.QuoteCurrencies.All(c => c.Code == c.Code.ToUpperInvariant())),
            A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }
}