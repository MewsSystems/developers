using ApplicationLayer.Commands.Currencies.DeleteCurrency;
using DomainLayer.Aggregates.ExchangeRateAggregate;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.ValueObjects;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for DeleteCurrencyCommandHandler.
/// Tests validation logic for provider usage and exchange rate dependencies.
/// </summary>
public class DeleteCurrencyCommandHandlerTests : TestBase
{
    private readonly DeleteCurrencyCommandHandler _handler;

    public DeleteCurrencyCommandHandlerTests()
    {
        _handler = new DeleteCurrencyCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteCurrencyCommandHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithUnusedCurrency_ShouldDeleteSuccessfully()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: false);
        var currency = Currency.FromCode("USD", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: No providers using this currency
        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider>());

        // Mock: No other currencies (so no exchange rates to check)
        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { currency });

        MockCurrencyRepository
            .Setup(x => x.DeleteAsync(currency, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(currency, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WithNonExistentCurrency_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 999, Force: false);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCurrencyUsedByProviders_WithoutForce_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: false);
        var currency = Currency.FromCode("EUR", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: Provider using this currency as base
        var provider = ExchangeRateProvider.Create(
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1); // Using our currency

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider> { provider });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("used as base currency");
        result.Error.Should().Contain("Force=true");

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCurrencyUsedByProviders_WithForce_ShouldDeleteSuccessfully()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: true);
        var currency = Currency.FromCode("EUR", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: Provider using this currency as base
        var provider = ExchangeRateProvider.Create(
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1);

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider> { provider });

        // Mock: No other currencies for rate checking
        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { currency });

        MockCurrencyRepository
            .Setup(x => x.DeleteAsync(currency, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(currency, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WithCurrencyHavingExchangeRates_WithoutForce_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: false);
        var currency = Currency.FromCode("USD", id: 1);
        var otherCurrency = Currency.FromCode("EUR", id: 2);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: No providers using this currency
        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider>());

        // Mock: Other currencies exist
        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { currency, otherCurrency });

        // Mock: Exchange rates exist between USD and EUR
        var rate = ExchangeRate.Create(
            providerId: 1,
            baseCurrencyId: 1,
            targetCurrencyId: 2,
            multiplier: 1,
            rate: 1.0850m,
            validDate: new DateOnly(2025, 11, 6));

        MockExchangeRateRepository
            .Setup(x => x.GetHistoryAsync(
                1, 2,
                It.IsAny<DateOnly>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRate>)new List<ExchangeRate> { rate });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("associated exchange rates");
        result.Error.Should().Contain("Force=true");

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCurrencyHavingExchangeRates_WithForce_ShouldDeleteSuccessfully()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: true);
        var currency = Currency.FromCode("USD", id: 1);
        var otherCurrency = Currency.FromCode("EUR", id: 2);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: No providers using this currency
        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider>());

        // Mock: Other currencies exist
        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { currency, otherCurrency });

        // Mock: Exchange rates exist (but Force=true bypasses check)
        var rate = ExchangeRate.Create(
            providerId: 1,
            baseCurrencyId: 1,
            targetCurrencyId: 2,
            multiplier: 1,
            rate: 1.0850m,
            validDate: new DateOnly(2025, 11, 6));

        MockExchangeRateRepository
            .Setup(x => x.GetHistoryAsync(
                1, 2,
                It.IsAny<DateOnly>(),
                It.IsAny<DateOnly>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRate>)new List<ExchangeRate> { rate });

        MockCurrencyRepository
            .Setup(x => x.DeleteAsync(currency, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(currency, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: false);
        var currency = Currency.FromCode("GBP", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database connection error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to delete currency");

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepositories()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: false);
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var currency = Currency.FromCode("JPY", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, token))
            .ReturnsAsync(currency);

        MockProviderRepository
            .Setup(x => x.GetAllAsync(token))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider>());

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(token))
            .ReturnsAsync(new List<Currency> { currency });

        MockCurrencyRepository
            .Setup(x => x.DeleteAsync(currency, token))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(token))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, token);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify token was passed through all repository calls
        MockCurrencyRepository.Verify(
            x => x.GetByIdAsync(1, token),
            Times.Once);

        MockProviderRepository.Verify(
            x => x.GetAllAsync(token),
            Times.Once);

        MockCurrencyRepository.Verify(
            x => x.GetAllAsync(token),
            Times.Once);

        MockCurrencyRepository.Verify(
            x => x.DeleteAsync(currency, token),
            Times.Once);

        MockUnitOfWork.Verify(
            x => x.SaveChangesAsync(token),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleProvidersUsingCurrency_ShouldReportCorrectCount()
    {
        // Arrange
        var command = new DeleteCurrencyCommand(CurrencyId: 1, Force: false);
        var currency = Currency.FromCode("EUR", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: Multiple providers using this currency
        var provider1 = ExchangeRateProvider.Create(
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1);

        var provider2 = ExchangeRateProvider.Create(
            name: "Romanian National Bank",
            code: "BNR",
            url: "https://api.bnr.ro",
            baseCurrencyId: 1);

        var provider3 = ExchangeRateProvider.Create(
            name: "Czech National Bank",
            code: "CNB",
            url: "https://api.cnb.cz",
            baseCurrencyId: 2); // Different currency

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider> { provider1, provider2, provider3 });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("2 provider(s)"); // Only 2 use this currency

        VerifySaveChangesNotCalled();
    }
}
