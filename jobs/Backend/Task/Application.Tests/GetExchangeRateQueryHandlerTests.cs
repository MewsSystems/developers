using Application.UseCases.ExchangeRates;
using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Errors;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

public class GetExchangeRateQueryHandlerTests
{
    private readonly Mock<ILogger<GetExchangeRateQueryHandler>> _mockLogger;
    private readonly Mock<IAvailableCurrencies> _mockCurrencyData;
    private readonly Mock<IExchangeRateProvider> _mockExchangeRateProvider;
    private readonly GetExchangeRateQueryHandler _handler;

    public GetExchangeRateQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetExchangeRateQueryHandler>>();
        _mockCurrencyData = new Mock<IAvailableCurrencies>();
        _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();

        _handler = new GetExchangeRateQueryHandler(
            _mockLogger.Object,
            _mockCurrencyData.Object,
            _mockExchangeRateProvider.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSourceCurrencyNotFound()
    {
        // Arrange
        var query = new GetExchangeRateQuery("en", "XYZ", "USD");

        _mockCurrencyData
            .Setup(m => m.GetCurrencyWithCode("XYZ"))
            .Returns((Currency)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.IsType<CurrencyNotFound>(result.Errors[0]);
        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExchangeRateNotFound()
    {
        // Arrange
        var query = new GetExchangeRateQuery("en", "USD", "EUR");


        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");

        _mockCurrencyData
            .Setup(m => m.GetCurrencyWithCode("USD"))
            .Returns(sourceCurrency);

        _mockCurrencyData
            .Setup(m => m.GetCurrencyWithCode("EUR"))
            .Returns(targetCurrency);

        _mockExchangeRateProvider
            .Setup(m => m.GetExchangeRate(sourceCurrency, targetCurrency, "en"))
            .ReturnsAsync((ExchangeRate)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Single(result.Errors);
        Assert.IsType<ExchangeRateNotFound>(result.Errors[0]);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenExchangeRateIsFound()
    {
        // Arrange
        var query = new GetExchangeRateQuery("en", "USD", "EUR");

        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("EUR");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 1.2m);

        _mockCurrencyData
            .Setup(m => m.GetCurrencyWithCode("USD"))
            .Returns(sourceCurrency);

        _mockCurrencyData
            .Setup(m => m.GetCurrencyWithCode("EUR"))
            .Returns(targetCurrency);

        _mockExchangeRateProvider
            .Setup(m => m.GetExchangeRate(sourceCurrency, targetCurrency, "en"))
            .ReturnsAsync(exchangeRate);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(exchangeRate, result.Value.ExchangeRate);
    }
}
