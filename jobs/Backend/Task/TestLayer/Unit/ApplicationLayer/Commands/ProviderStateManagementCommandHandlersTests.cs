using ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;
using DomainLayer.Aggregates.ExchangeRateAggregate;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Exceptions;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for provider state management commands:
/// ActivateProvider, DeactivateProvider, and DeleteProvider.
/// </summary>
public class ProviderStateManagementCommandHandlersTests : TestBase
{
    #region ActivateProviderCommandHandler Tests

    [Fact]
    public async Task ActivateProvider_WithExistingProvider_ShouldActivateSuccessfully()
    {
        // Arrange
        var handler = new ActivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ActivateProviderCommandHandler>().Object);

        var command = new ActivateProviderCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test Provider", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockProviderRepository.Verify(
            x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task ActivateProvider_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ActivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ActivateProviderCommandHandler>().Object);

        var command = new ActivateProviderCommand(ProviderId: 999);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");

        MockProviderRepository.Verify(
            x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task ActivateProvider_WhenProviderQuarantined_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ActivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ActivateProviderCommandHandler>().Object);

        var command = new ActivateProviderCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        // Quarantine the provider
        for (int i = 0; i < 5; i++)
        {
            provider.RecordFailedFetch("Test failure");
        }

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("quarantine");

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task ActivateProvider_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ActivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ActivateProviderCommandHandler>().Object);

        var command = new ActivateProviderCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("error occurred");
    }

    [Fact]
    public async Task ActivateProvider_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new ActivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ActivateProviderCommandHandler>().Object);

        var command = new ActivateProviderCommand(1);
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(provider);
        MockProviderRepository.Setup(x => x.UpdateAsync(provider, token)).Returns(Task.CompletedTask);
        MockUnitOfWork.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(1);

        // Act
        await handler.Handle(command, token);

        // Assert
        MockProviderRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockProviderRepository.Verify(x => x.UpdateAsync(provider, token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    #endregion

    #region DeactivateProviderCommandHandler Tests

    [Fact]
    public async Task DeactivateProvider_WithExistingProvider_ShouldDeactivateSuccessfully()
    {
        // Arrange
        var handler = new DeactivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeactivateProviderCommandHandler>().Object);

        var command = new DeactivateProviderCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test Provider", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockProviderRepository.Verify(
            x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task DeactivateProvider_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new DeactivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeactivateProviderCommandHandler>().Object);

        var command = new DeactivateProviderCommand(ProviderId: 999);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");

        MockProviderRepository.Verify(
            x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task DeactivateProvider_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new DeactivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeactivateProviderCommandHandler>().Object);

        var command = new DeactivateProviderCommand(1);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("error occurred");
    }

    [Fact]
    public async Task DeactivateProvider_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new DeactivateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeactivateProviderCommandHandler>().Object);

        var command = new DeactivateProviderCommand(1);
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(provider);
        MockProviderRepository.Setup(x => x.UpdateAsync(provider, token)).Returns(Task.CompletedTask);
        MockUnitOfWork.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(1);

        // Act
        await handler.Handle(command, token);

        // Assert
        MockProviderRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockProviderRepository.Verify(x => x.UpdateAsync(provider, token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    #endregion

    #region DeleteProviderCommandHandler Tests

    [Fact]
    public async Task DeleteProvider_WithoutExchangeRates_ShouldDeleteSuccessfully()
    {
        // Arrange
        var handler = new DeleteProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteProviderCommandHandler>().Object);

        var command = new DeleteProviderCommand(ProviderId: 1, Force: false);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // No exchange rates
        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateAsync(1, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRate>)new List<ExchangeRate>());

        MockProviderRepository
            .Setup(x => x.DeleteAsync(provider, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockProviderRepository.Verify(
            x => x.DeleteAsync(provider, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task DeleteProvider_WithExchangeRatesWithoutForce_ShouldReturnFailure()
    {
        // Arrange
        var handler = new DeleteProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteProviderCommandHandler>().Object);

        var command = new DeleteProviderCommand(ProviderId: 1, Force: false);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        var rate = ExchangeRate.Create(
            providerId: 1,
            baseCurrencyId: 1,
            targetCurrencyId: 2,
            multiplier: 1,
            rate: 1.0850m,
            validDate: new DateOnly(2025, 11, 6));

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateAsync(1, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRate>)new List<ExchangeRate> { rate });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("has associated exchange rates");
        result.Error.Should().Contain("Force=true");

        MockProviderRepository.Verify(
            x => x.DeleteAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task DeleteProvider_WithExchangeRatesWithForce_ShouldDeleteBoth()
    {
        // Arrange
        var handler = new DeleteProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteProviderCommandHandler>().Object);

        var command = new DeleteProviderCommand(ProviderId: 1, Force: true);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        var rate1 = ExchangeRate.Create(1, 1, 2, 1, 1.0850m, new DateOnly(2025, 11, 6));
        var rate2 = ExchangeRate.Create(1, 1, 3, 1, 0.8450m, new DateOnly(2025, 11, 6));

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateAsync(1, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRate>)new List<ExchangeRate> { rate1, rate2 });

        MockExchangeRateRepository
            .Setup(x => x.DeleteAsync(It.IsAny<ExchangeRate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockProviderRepository
            .Setup(x => x.DeleteAsync(provider, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify both rates were deleted
        MockExchangeRateRepository.Verify(
            x => x.DeleteAsync(It.IsAny<ExchangeRate>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        // Verify provider was deleted
        MockProviderRepository.Verify(
            x => x.DeleteAsync(provider, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task DeleteProvider_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new DeleteProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteProviderCommandHandler>().Object);

        var command = new DeleteProviderCommand(ProviderId: 999, Force: false);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task DeleteProvider_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new DeleteProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteProviderCommandHandler>().Object);

        var command = new DeleteProviderCommand(1, false);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateAsync(1, It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to delete provider");
    }

    [Fact]
    public async Task DeleteProvider_WithCancellationToken_ShouldPassTokenToRepositories()
    {
        // Arrange
        var handler = new DeleteProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<DeleteProviderCommandHandler>().Object);

        var command = new DeleteProviderCommand(1, false);
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(provider);
        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateAsync(1, It.IsAny<DateOnly>(), token))
            .ReturnsAsync((IEnumerable<ExchangeRate>)new List<ExchangeRate>());
        MockProviderRepository.Setup(x => x.DeleteAsync(provider, token)).Returns(Task.CompletedTask);
        MockUnitOfWork.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(1);

        // Act
        await handler.Handle(command, token);

        // Assert
        MockProviderRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockExchangeRateRepository.Verify(
            x => x.GetByProviderAndDateAsync(1, It.IsAny<DateOnly>(), token),
            Times.Once);
        MockProviderRepository.Verify(x => x.DeleteAsync(provider, token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    #endregion
}
