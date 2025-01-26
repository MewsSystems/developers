using Application.UseCases.ExchangeRates;
using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Errors;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

public class GetDailyExchangeRateQueryHandlerTests
{
    private readonly Mock<ILogger<GetDailyExchangeRateQueryHandler>> _mockLogger;
    private readonly Mock<IAvailableCurrencies> _mockCurrencyData;
    private readonly Mock<IExchangeRateProvider> _mockExchangeRateProvider;
    private readonly GetDailyExchangeRateQueryHandler _handler;

    public GetDailyExchangeRateQueryHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GetDailyExchangeRateQueryHandler>>();
        _mockCurrencyData = new Mock<IAvailableCurrencies>();
        _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();

        _handler = new GetDailyExchangeRateQueryHandler(
            _mockLogger.Object,
            _mockCurrencyData.Object,
            _mockExchangeRateProvider.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCurrencyNotFound()
    {
        // Arrange
        var query = new GetDailyExchangeRateQuery("XYZ", "en");
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
    public async Task Handle_ShouldReturnSuccess_WhenCurrencyIsFound()
    {
        // Arrange
        var query = new GetDailyExchangeRateQuery("USD", "en");
        var currency = new Currency("USD");
        var exchangeRates = new List<ExchangeRate> 
        { 
            new ExchangeRate(currency, new Currency("EUR"), 1.2m) 
        };

        _mockCurrencyData
            .Setup(m => m.GetCurrencyWithCode("USD"))
            .Returns(currency);

        _mockExchangeRateProvider
            .Setup(m => m.GetDailyExchangeRates(currency, "en"))
            .ReturnsAsync(exchangeRates);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(exchangeRates, result.Value.ExchangeRates);
    }
}