using ApplicationLayer.Commands.Currencies.CreateCurrency;
using DomainLayer.ValueObjects;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for CreateCurrencyCommandHandler.
/// Tests command flow through ApplicationLayer to DataLayer boundary.
/// </summary>
public class CreateCurrencyCommandHandlerTests : TestBase
{
    private readonly CreateCurrencyCommandHandler _handler;

    public CreateCurrencyCommandHandlerTests()
    {
        _handler = new CreateCurrencyCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<CreateCurrencyCommandHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithValidNewCurrency_ShouldCreateCurrencySuccessfully()
    {
        // Arrange
        var command = new CreateCurrencyCommand("USD");
        var createdCurrency = Currency.FromCode("USD", id: 123);

        // Mock: Currency doesn't exist on first call, exists with ID on second call (after save)
        var callCount = 0;
        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? null : createdCurrency;
            });

        // Mock: Add async
        MockCurrencyRepository
            .Setup(x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(123); // ID assigned by "database"

        // Verify repository interactions - GetByCodeAsync called twice now (check + query back for ID)
        MockCurrencyRepository.Verify(
            x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        MockCurrencyRepository.Verify(
            x => x.AddAsync(It.Is<Currency>(c => c.Code == "USD"), It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WithExistingCurrency_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateCurrencyCommand("EUR");
        var existingCurrency = Currency.FromCode("EUR", id: 1);

        // Mock: Currency already exists
        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCurrency);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");

        // Verify AddAsync was never called
        MockCurrencyRepository.Verify(
            x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("U")]
    [InlineData("USDD")]
    [InlineData("12")]
    public async Task Handle_WithInvalidCurrencyCode_ShouldReturnFailure(string invalidCode)
    {
        // Arrange
        var command = new CreateCurrencyCommand(invalidCode);

        // Mock: Currency doesn't exist
        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync(invalidCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeEmpty();

        // Verify AddAsync was never called
        MockCurrencyRepository.Verify(
            x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateCurrencyCommand("GBP");

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("GBP", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        MockCurrencyRepository
            .Setup(x => x.AddAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to create currency");

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var command = new CreateCurrencyCommand("JPY");
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var createdCurrency = Currency.FromCode("JPY", id: 456);

        // Mock: Currency doesn't exist on first call, exists with ID on second call (after save)
        var callCount = 0;
        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("JPY", token))
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1 ? null : createdCurrency;
            });

        MockCurrencyRepository
            .Setup(x => x.AddAsync(It.IsAny<Currency>(), token))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(token))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, token);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify token was passed through - GetByCodeAsync called twice now (check + query back for ID)
        MockCurrencyRepository.Verify(
            x => x.GetByCodeAsync("JPY", token),
            Times.Exactly(2));

        MockCurrencyRepository.Verify(
            x => x.AddAsync(It.IsAny<Currency>(), token),
            Times.Once);

        MockUnitOfWork.Verify(
            x => x.SaveChangesAsync(token),
            Times.Once);
    }
}
