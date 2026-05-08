using ExchangeRateUpdater.Api.Controllers.V1;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Models;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using FluentValidation;

namespace ExchangeRateUpdater.Tests.Unit.Controllers;

public class ExchangeRateControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<ExchangeRateController>> _loggerMock;
    private readonly ExchangeRateController _controller;

    public ExchangeRateControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<ExchangeRateController>>();
        _controller = new ExchangeRateController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetExchangeRate_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var sourceCurrency = "USD";
        var targetCurrency = "CZK";
        var date = "2024-03-20";
        var expectedResponse = new ExchangeRateResponse
        {
            SourceCurrency = sourceCurrency,
            TargetCurrency = targetCurrency,
            Rate = 23.5m,
            Date = date
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetExchangeRateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetExchangeRate(sourceCurrency, targetCurrency, date);

        // Assert
        var okResult = result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;
        var response = okResult.Value.Should()
            .BeOfType<ExchangeRateResponse>()
            .Subject;
        response.Should()
            .BeEquivalentTo(expectedResponse);

        _mediatorMock.Verify(m => m.Send(It.Is<GetExchangeRateQuery>(q =>
                    q.SourceCurrency == sourceCurrency &&
                    q.TargetCurrency == targetCurrency &&
                    q.Date == date),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExchangeRate_InvalidCurrencyPair_ReturnsBadRequest()
    {
        // Arrange
        var sourceCurrency = "INVALID";
        var targetCurrency = "CZK";
        var date = "2024-03-20";
        var errorMessage = "Invalid currency pair";

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetExchangeRateQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(errorMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _controller.GetExchangeRate(sourceCurrency, targetCurrency, date));

        exception.Message.Should()
            .Be(errorMessage);
    }

    [Fact]
    public async Task GetBatchExchangeRates_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new BatchRateRequest
        {
            Date = "2024-03-20",
            CurrencyPairs = new[] { "USD/CZK", "EUR/CZK" }
        };

        var expectedResponse = new BatchExchangeRateResponse
        {
            Date = request.Date,
            Rates = new List<ExchangeRateResponse>
            {
                new() { SourceCurrency = "USD", TargetCurrency = "CZK", Rate = 23.5m },
                new() { SourceCurrency = "EUR", TargetCurrency = "CZK", Rate = 25.5m }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBatchExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetBatchExchangeRates(request);

        // Assert
        var okResult = result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Subject;
        var response = okResult.Value.Should()
            .BeOfType<BatchExchangeRateResponse>()
            .Subject;
        response.Should()
            .BeEquivalentTo(expectedResponse);

        _mediatorMock.Verify(m => m.Send(It.Is<GetBatchExchangeRatesQuery>(q =>
                    q.Request == request),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetBatchExchangeRates_EmptyRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new BatchRateRequest
        {
            Date = "2024-03-20",
            CurrencyPairs = Array.Empty<string>()
        };
        var errorMessage = "Currency pairs cannot be empty";

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBatchExchangeRatesQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(errorMessage));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _controller.GetBatchExchangeRates(request));

        exception.Message.Should()
            .Be(errorMessage);
    }
}