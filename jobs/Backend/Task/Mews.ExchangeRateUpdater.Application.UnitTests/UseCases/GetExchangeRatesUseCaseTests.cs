using Mews.ExchangeRateUpdater.Application.Exceptions;
using Mews.ExchangeRateUpdater.Application.UseCases;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRateUpdater.Application.UnitTests.UseCases;

public class GetExchangeRatesUseCaseTests
{
    private readonly Mock<ILogger<GetExchangeRatesUseCase>> _loggerMock = new();
    private readonly Mock<IExchangeRateRepository> _repoMock = new();

    [Fact]
    public async Task ThrowsException_WhenNoRatesAvailable()
    {
        // Arrange
        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var useCase = new GetExchangeRatesUseCase(_repoMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NoDataForTodayException>(() => useCase.ExecuteAsync(new[] { new Currency("USD") }, CancellationToken.None));
    }

    [Fact]
    public async Task ReturnsRates_WhenRatesAvailable()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        var expectedRates = new List<ExchangeRate> { new(new Currency("USD"), new Currency("CZK"), 24.5m) };

        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repoMock.Setup(r => r.GetRatesAsync(It.IsAny<DateTime>(), currencies, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRates);

        var useCase = new GetExchangeRatesUseCase(_repoMock.Object, _loggerMock.Object);

        // Act
        var result = await useCase.ExecuteAsync(currencies, CancellationToken.None);

        // Assert
        Assert.Equal(expectedRates.Count, result.Count());
        Assert.Equal(expectedRates.First().Value, result.First().Value);
    }
    
    [Fact]
    public async Task DoesNotCallGetRates_WhenNoRatesAvailable()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };

        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var useCase = new GetExchangeRatesUseCase(_repoMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NoDataForTodayException>(() => useCase.ExecuteAsync(currencies, CancellationToken.None));

        // Assert
        _repoMock.Verify(r => r.GetRatesAsync(It.IsAny<DateTime>(), It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}