using ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;
using ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;
using ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Exceptions;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for Provider maintenance command handlers.
/// Tests ResetProviderHealth, TriggerManualFetch, and UpdateProviderConfiguration.
/// </summary>
public class ProviderMaintenanceCommandHandlersTests : TestBase
{
    #region ResetProviderHealthCommandHandler Tests

    [Fact]
    public async Task ResetProviderHealth_WithValidProvider_ShouldResetHealthSuccessfully()
    {
        // Arrange
        var handler = new ResetProviderHealthCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ResetProviderHealthCommandHandler>().Object);

        var command = new ResetProviderHealthCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test Provider", "TEST", "https://test.com", 1);

        // Simulate failures
        provider.RecordFailedFetch("Error 1");
        provider.RecordFailedFetch("Error 2");
        provider.RecordFailedFetch("Error 3");

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
        provider.ConsecutiveFailures.Should().Be(0);
        provider.LastFailedFetch.Should().BeNull();

        MockProviderRepository.Verify(
            x => x.UpdateAsync(provider, It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task ResetProviderHealth_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ResetProviderHealthCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ResetProviderHealthCommandHandler>().Object);

        var command = new ResetProviderHealthCommand(ProviderId: 999);

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
    public async Task ResetProviderHealth_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new ResetProviderHealthCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ResetProviderHealthCommandHandler>().Object);

        var command = new ResetProviderHealthCommand(ProviderId: 1);
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
    public async Task ResetProviderHealth_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new ResetProviderHealthCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ResetProviderHealthCommandHandler>().Object);

        var command = new ResetProviderHealthCommand(ProviderId: 1);
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

    #region TriggerManualFetchCommandHandler Tests

    [Fact]
    public async Task TriggerManualFetch_WithActiveProvider_ShouldEnqueueJobSuccessfully()
    {
        // Arrange
        var handler = new TriggerManualFetchCommandHandler(
            MockUnitOfWork.Object,
            MockBackgroundJobService.Object,
            CreateMockLogger<TriggerManualFetchCommandHandler>().Object);

        var command = new TriggerManualFetchCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test Provider", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockBackgroundJobService
            .Setup(x => x.EnqueueFetchRatesJob("TEST"))
            .Returns("job-12345");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("job-12345");

        MockBackgroundJobService.Verify(
            x => x.EnqueueFetchRatesJob("TEST"),
            Times.Once);
    }

    [Fact]
    public async Task TriggerManualFetch_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new TriggerManualFetchCommandHandler(
            MockUnitOfWork.Object,
            MockBackgroundJobService.Object,
            CreateMockLogger<TriggerManualFetchCommandHandler>().Object);

        var command = new TriggerManualFetchCommand(ProviderId: 999);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");

        MockBackgroundJobService.Verify(
            x => x.EnqueueFetchRatesJob(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerManualFetch_WithInactiveProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new TriggerManualFetchCommandHandler(
            MockUnitOfWork.Object,
            MockBackgroundJobService.Object,
            CreateMockLogger<TriggerManualFetchCommandHandler>().Object);

        var command = new TriggerManualFetchCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        // Deactivate the provider
        provider.Deactivate();

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not active");

        MockBackgroundJobService.Verify(
            x => x.EnqueueFetchRatesJob(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerManualFetch_WithQuarantinedProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new TriggerManualFetchCommandHandler(
            MockUnitOfWork.Object,
            MockBackgroundJobService.Object,
            CreateMockLogger<TriggerManualFetchCommandHandler>().Object);

        var command = new TriggerManualFetchCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        // Quarantine the provider (5 consecutive failures)
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
        result.Error.Should().Contain("Quarantined");

        MockBackgroundJobService.Verify(
            x => x.EnqueueFetchRatesJob(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task TriggerManualFetch_WhenBackgroundJobServiceThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new TriggerManualFetchCommandHandler(
            MockUnitOfWork.Object,
            MockBackgroundJobService.Object,
            CreateMockLogger<TriggerManualFetchCommandHandler>().Object);

        var command = new TriggerManualFetchCommand(ProviderId: 1);
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockBackgroundJobService
            .Setup(x => x.EnqueueFetchRatesJob("TEST"))
            .Throws(new InvalidOperationException("Job queue error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to trigger manual fetch");
    }

    [Fact]
    public async Task TriggerManualFetch_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new TriggerManualFetchCommandHandler(
            MockUnitOfWork.Object,
            MockBackgroundJobService.Object,
            CreateMockLogger<TriggerManualFetchCommandHandler>().Object);

        var command = new TriggerManualFetchCommand(ProviderId: 1);
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 1);

        MockProviderRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(provider);
        MockBackgroundJobService.Setup(x => x.EnqueueFetchRatesJob("TEST")).Returns("job-123");

        // Act
        await handler.Handle(command, token);

        // Assert
        MockProviderRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
    }

    #endregion

    #region UpdateProviderConfigurationCommandHandler Tests

    [Fact]
    public async Task UpdateProviderConfiguration_WithNameAndUrl_ShouldUpdateSuccessfully()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            Name: "Updated Provider Name",
            Url: "https://updated.com");

        var provider = ExchangeRateProvider.Create(
            "Old Name",
            "TEST",
            "https://old.com",
            1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        provider.Name.Should().Be("Updated Provider Name");
        provider.Url.Should().Be("https://updated.com");

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WithAuthentication_ShouldUpdateSuccessfully()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            RequiresAuthentication: true,
            ApiKeyVaultReference: "new-vault-key");

        var provider = ExchangeRateProvider.Create(
            "Test Provider",
            "TEST",
            "https://test.com",
            1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        provider.RequiresAuthentication.Should().BeTrue();
        provider.ApiKeyVaultReference.Should().Be("new-vault-key");

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WithAllFields_ShouldUpdateSuccessfully()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            Name: "New Name",
            Url: "https://new.com",
            RequiresAuthentication: true,
            ApiKeyVaultReference: "vault-key");

        var provider = ExchangeRateProvider.Create(
            "Old Name",
            "TEST",
            "https://old.com",
            1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        provider.Name.Should().Be("New Name");
        provider.Url.Should().Be("https://new.com");
        provider.RequiresAuthentication.Should().BeTrue();
        provider.ApiKeyVaultReference.Should().Be("vault-key");

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 999,
            Name: "New Name");

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

    [Theory]
    [InlineData("", "https://test.com")] // Empty name
    [InlineData("Name", "")] // Empty URL
    public async Task UpdateProviderConfiguration_WithInvalidData_ShouldReturnFailure(
        string name,
        string url)
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            Name: name,
            Url: url);

        var provider = ExchangeRateProvider.Create(
            "Old Name",
            "TEST",
            "https://old.com",
            1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeEmpty();

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WithAuthRequiredButNoApiKey_ShouldReturnFailure()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            RequiresAuthentication: true,
            ApiKeyVaultReference: null); // Invalid: auth required but no key

        var provider = ExchangeRateProvider.Create(
            "Test",
            "TEST",
            "https://test.com",
            1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("API key");

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WhenRepositoryThrows_ShouldReturnFailure()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            Name: "New Name");

        var provider = ExchangeRateProvider.Create(
            "Old Name",
            "TEST",
            "https://test.com",
            1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to update provider configuration");
    }

    [Fact]
    public async Task UpdateProviderConfiguration_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new UpdateProviderConfigurationCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<UpdateProviderConfigurationCommandHandler>().Object);

        var command = new UpdateProviderConfigurationCommand(
            ProviderId: 1,
            Name: "New Name");

        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var provider = ExchangeRateProvider.Create("Old Name", "TEST", "https://test.com", 1);

        MockProviderRepository.Setup(x => x.GetByIdAsync(1, token)).ReturnsAsync(provider);
        MockUnitOfWork.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(1);

        // Act
        await handler.Handle(command, token);

        // Assert
        MockProviderRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    #endregion
}
