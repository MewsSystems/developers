using Mews.ExchangeRateUpdater.Application.Exceptions;
using Mews.ExchangeRateUpdater.Application.UseCases;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mews.ExchangeRateUpdater.Application.UnitTests.UseCases;

public class FetchExchangeRatesUseCaseTests
{
    private readonly Mock<ILogger<FetchExchangeRatesUseCase>> _loggerMock = new();
    private readonly Mock<ICnbService> _clientMock = new();
    private readonly Mock<IExchangeRateRepository> _repoMock = new();

    [Fact]
    public async Task ThrowsException_WhenRatesAlreadyExist_AndNotForced()
    {
        // Arrange
        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var useCase = new FetchExchangeRatesUseCase(_clientMock.Object, _repoMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<RatesAlreadyExistException>(() => useCase.ExecuteAsync(CancellationToken.None, false));
    }
    
    [Fact]
    public async Task ForcesFetch_WhenRatesAlreadyExist_AndForceUpdateIsTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _clientMock.Setup(c => c.GetLatestRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ExchangeRate>
            {
                new(new Currency("EUR"), new Currency("CZK"), 25.0m)
            });

        var useCase = new FetchExchangeRatesUseCase(_clientMock.Object, _repoMock.Object, _loggerMock.Object);

        // Act
        await useCase.ExecuteAsync(CancellationToken.None, forceUpdate: true);

        // Assert
        _repoMock.Verify(r => r.UpsertRatesAsync(It.IsAny<IEnumerable<ExchangeRate>>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ThrowsException_WhenNoRatesFetched()
    {
        // Arrange
        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _clientMock.Setup(c => c.GetLatestRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

        var useCase = new FetchExchangeRatesUseCase(_clientMock.Object, _repoMock.Object, _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EmptyRatesFetchedException>(() => useCase.ExecuteAsync(CancellationToken.None, false));
    }

    [Fact]
    public async Task CallsUpsert_WhenRatesFetchedSuccessfully()
    {
        // Arrange
        _repoMock.Setup(r => r.HasRatesForDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _clientMock.Setup(c => c.GetLatestRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ExchangeRate>
            {
                new(new Currency("USD"), new Currency("CZK"), 24.5m)
            });

        var useCase = new FetchExchangeRatesUseCase(_clientMock.Object, _repoMock.Object, _loggerMock.Object);

        // Act
        await useCase.ExecuteAsync(CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.UpsertRatesAsync(It.IsAny<IEnumerable<ExchangeRate>>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}