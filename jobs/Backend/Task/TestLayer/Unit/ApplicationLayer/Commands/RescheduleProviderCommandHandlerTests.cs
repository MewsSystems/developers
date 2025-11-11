using ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;
using ApplicationLayer.Common.Interfaces;
using DomainLayer.Aggregates.ProviderAggregate;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for RescheduleProviderCommandHandler.
/// Tests provider rescheduling with time/timezone configuration and job scheduler integration.
/// </summary>
public class RescheduleProviderCommandHandlerTests : TestBase
{
    private readonly Mock<IBackgroundJobScheduler> _mockJobScheduler;
    private readonly RescheduleProviderCommandHandler _handler;

    public RescheduleProviderCommandHandlerTests()
    {
        _mockJobScheduler = new Mock<IBackgroundJobScheduler>();
        _handler = new RescheduleProviderCommandHandler(
            MockUnitOfWork.Object,
            _mockJobScheduler.Object,
            CreateMockLogger<RescheduleProviderCommandHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithValidProvider_ShouldRescheduleSuccessfully()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "ECB",
            UpdateTime: "14:30",
            TimeZone: "CET");

        var provider = ExchangeRateProvider.Reconstruct(
            id: 1,
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockJobScheduler
            .Setup(x => x.RescheduleProviderJobAsync("ECB", "14:30", "CET"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify configuration was updated
        provider.GetConfigurationValue("UpdateTime").Should().Be("14:30");
        provider.GetConfigurationValue("TimeZone").Should().Be("CET");

        // Verify repository operations
        MockProviderRepository.Verify(
            x => x.UpdateAsync(It.Is<ExchangeRateProvider>(p => p.Code == "ECB"), It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();

        // Verify job was rescheduled
        _mockJobScheduler.Verify(
            x => x.RescheduleProviderJobAsync("ECB", "14:30", "CET"),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentProvider_ShouldReturnFailure()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "UNKNOWN",
            UpdateTime: "14:30",
            TimeZone: "CET");

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("UNKNOWN", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("not found");

        MockProviderRepository.Verify(
            x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();

        _mockJobScheduler.Verify(
            x => x.RescheduleProviderJobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenJobSchedulerThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "ECB",
            UpdateTime: "14:30",
            TimeZone: "CET");

        var provider = ExchangeRateProvider.Reconstruct(
            id: 1,
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockJobScheduler
            .Setup(x => x.RescheduleProviderJobAsync("ECB", "14:30", "CET"))
            .ThrowsAsync(new InvalidOperationException("Hangfire error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to reschedule");

        // Configuration should still be updated
        VerifySaveChangesCalled();
    }

    [Theory]
    [InlineData("00:00", "UTC")]
    [InlineData("12:30", "CET")]
    [InlineData("23:59", "CEST")]
    [InlineData("06:15", "EET")]
    [InlineData("18:45", "Europe/Prague")]
    public async Task Handle_WithDifferentTimeZones_ShouldRescheduleSuccessfully(
        string updateTime,
        string timeZone)
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "TEST",
            UpdateTime: updateTime,
            TimeZone: timeZone);

        var provider = ExchangeRateProvider.Reconstruct(
            id: 1,
            name: "Test Provider",
            code: "TEST",
            url: "https://test.com",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("TEST", It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockJobScheduler
            .Setup(x => x.RescheduleProviderJobAsync("TEST", updateTime, timeZone))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        provider.GetConfigurationValue("UpdateTime").Should().Be(updateTime);
        provider.GetConfigurationValue("TimeZone").Should().Be(timeZone);

        _mockJobScheduler.Verify(
            x => x.RescheduleProviderJobAsync("TEST", updateTime, timeZone),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepositories()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "ECB",
            UpdateTime: "14:30",
            TimeZone: "CET");

        var cts = new CancellationTokenSource();
        var token = cts.Token;

        var provider = ExchangeRateProvider.Reconstruct(
            id: 1,
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", token))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), token))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(token))
            .ReturnsAsync(1);

        _mockJobScheduler
            .Setup(x => x.RescheduleProviderJobAsync("ECB", "14:30", "CET"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, token);

        // Assert
        result.IsSuccess.Should().BeTrue();

        MockProviderRepository.Verify(x => x.GetByCodeAsync("ECB", token), Times.Once);
        MockProviderRepository.Verify(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "ECB",
            UpdateTime: "14:30",
            TimeZone: "CET");

        var provider = ExchangeRateProvider.Reconstruct(
            id: 1,
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to reschedule");

        VerifySaveChangesNotCalled();

        _mockJobScheduler.Verify(
            x => x.RescheduleProviderJobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UpdatesExistingConfiguration_ShouldOverwritePreviousValues()
    {
        // Arrange
        var command = new RescheduleProviderCommand(
            ProviderCode: "ECB",
            UpdateTime: "18:00",
            TimeZone: "CEST");

        var provider = ExchangeRateProvider.Reconstruct(
            id: 1,
            name: "European Central Bank",
            code: "ECB",
            url: "https://api.ecb.europa.eu",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        // Set initial configuration
        provider.SetConfiguration("UpdateTime", "14:00");
        provider.SetConfiguration("TimeZone", "CET");

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockProviderRepository
            .Setup(x => x.UpdateAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockJobScheduler
            .Setup(x => x.RescheduleProviderJobAsync("ECB", "18:00", "CEST"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        provider.GetConfigurationValue("UpdateTime").Should().Be("18:00");
        provider.GetConfigurationValue("TimeZone").Should().Be("CEST");

        VerifySaveChangesCalled();

        _mockJobScheduler.Verify(
            x => x.RescheduleProviderJobAsync("ECB", "18:00", "CEST"),
            Times.Once);
    }
}
