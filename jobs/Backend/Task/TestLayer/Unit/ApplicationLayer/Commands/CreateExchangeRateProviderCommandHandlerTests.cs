using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.ValueObjects;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for CreateExchangeRateProviderCommandHandler.
/// Tests provider creation with code uniqueness and currency validation.
/// </summary>
public class CreateExchangeRateProviderCommandHandlerTests : TestBase
{
    private readonly CreateExchangeRateProviderCommandHandler _handler;

    public CreateExchangeRateProviderCommandHandlerTests()
    {
        _handler = new CreateExchangeRateProviderCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<CreateExchangeRateProviderCommandHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithValidNewProvider_ShouldCreateProviderSuccessfully()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            Name: "European Central Bank",
            Code: "ECB",
            Url: "https://api.ecb.europa.eu",
            BaseCurrencyId: 1,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null);

        var currency = Currency.FromCode("EUR", id: 1);
        var createdProvider = ExchangeRateProvider.Reconstruct(
            id: 10,
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

        // Mock: Provider code doesn't exist on first call, exists with ID on second call (after save)
        var callCount = 0;
        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? null : createdProvider;
            });

        // Mock: Currency exists
        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Mock: Add and SaveChanges
        MockProviderRepository
            .Setup(x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(10);

        MockProviderRepository.Verify(
            x => x.AddAsync(It.Is<ExchangeRateProvider>(p =>
                p.Code == "ECB" &&
                p.Name == "European Central Bank" &&
                p.BaseCurrencyId == 1),
                It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WithExistingProviderCode_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            Name: "European Central Bank",
            Code: "ECB",
            Url: "https://api.ecb.europa.eu",
            BaseCurrencyId: 1,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null);

        var existingProvider = ExchangeRateProvider.Create(
            "Existing ECB",
            "ECB",
            "https://existing.com",
            1);

        // Mock: Provider code already exists
        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("ECB", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProvider);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");

        MockProviderRepository.Verify(
            x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithNonExistentCurrency_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            Name: "Test Provider",
            Code: "TEST",
            Url: "https://test.com",
            BaseCurrencyId: 999,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null);

        // Mock: Provider code doesn't exist
        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("TEST", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Mock: Currency doesn't exist
        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Currency");
        result.Error.Should().Contain("not found");

        MockProviderRepository.Verify(
            x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithAuthenticationRequired_ShouldCreateProviderWithApiKey()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            Name: "Authenticated Provider",
            Code: "AUTH",
            Url: "https://auth.com",
            BaseCurrencyId: 1,
            RequiresAuthentication: true,
            ApiKeyVaultReference: "vault-secret-key");

        var currency = Currency.FromCode("USD", id: 1);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("AUTH", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        MockProviderRepository
            .Setup(x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Callback<ExchangeRateProvider, CancellationToken>((provider, ct) =>
            {
                typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);
            })
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify provider was created with authentication settings
        MockProviderRepository.Verify(
            x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            Name: "Test Provider",
            Code: "TEST",
            Url: "https://test.com",
            BaseCurrencyId: 1,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null);

        var currency = Currency.FromCode("USD", id: 1);

        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("TEST", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        MockProviderRepository
            .Setup(x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("error occurred");

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepositories()
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            "Provider",
            "PRV",
            "https://provider.com",
            1,
            false,
            null);

        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var currency = Currency.FromCode("USD", id: 1);
        var createdProvider = ExchangeRateProvider.Reconstruct(
            id: 100,
            name: "Provider",
            code: "PRV",
            url: "https://provider.com",
            baseCurrencyId: 1,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        // Mock: Provider code doesn't exist on first call, exists with ID on second call (after save)
        var callCount = 0;
        MockProviderRepository
            .Setup(x => x.GetByCodeAsync("PRV", token))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? null : createdProvider;
            });

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, token))
            .ReturnsAsync(currency);

        MockProviderRepository
            .Setup(x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), token))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(token))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, token);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // GetByCodeAsync called twice now (check + query back for ID)
        MockProviderRepository.Verify(x => x.GetByCodeAsync("PRV", token), Times.Exactly(2));
        MockCurrencyRepository.Verify(x => x.GetByIdAsync(1, token), Times.Once);
        MockProviderRepository.Verify(x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), token), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChangesAsync(token), Times.Once);
    }

    [Theory]
    [InlineData("ECB", "European Central Bank", 1)] // Standard case
    [InlineData("CNB", "Czech National Bank", 2)]    // Different currency
    [InlineData("BNR", "Romanian National Bank", 3)] // Another provider
    public async Task Handle_WithDifferentValidProviders_ShouldCreateSuccessfully(
        string code,
        string name,
        int currencyId)
    {
        // Arrange
        var command = new CreateExchangeRateProviderCommand(
            name,
            code,
            $"https://{code.ToLower()}.com",
            currencyId,
            false,
            null);

        var currency = Currency.FromCode("CUR", id: currencyId);
        var createdProvider = ExchangeRateProvider.Reconstruct(
            id: currencyId,
            name: name,
            code: code,
            url: $"https://{code.ToLower()}.com",
            baseCurrencyId: currencyId,
            requiresAuthentication: false,
            apiKeyVaultReference: null,
            isActive: true,
            lastSuccessfulFetch: null,
            lastFailedFetch: null,
            consecutiveFailures: 0,
            created: DateTimeOffset.UtcNow,
            modified: null);

        // Mock: Provider code doesn't exist on first call, exists with ID on second call (after save)
        var callCount = 0;
        MockProviderRepository
            .Setup(x => x.GetByCodeAsync(code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? null : createdProvider;
            });

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(currencyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        MockProviderRepository
            .Setup(x => x.AddAsync(It.IsAny<ExchangeRateProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(currencyId);
    }
}
