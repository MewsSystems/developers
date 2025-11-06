using ApplicationLayer.Commands.ExchangeRates.BulkUpsertExchangeRates;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for BulkUpsertExchangeRatesCommandHandler.
/// Tests bulk insert/update operations for exchange rates.
/// </summary>
public class BulkUpsertExchangeRatesCommandHandlerTests : TestBase
{
    private readonly BulkUpsertExchangeRatesCommandHandler _handler;

    public BulkUpsertExchangeRatesCommandHandlerTests()
    {
        _handler = new BulkUpsertExchangeRatesCommandHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<BulkUpsertExchangeRatesCommandHandler>().Object);
    }

    [Fact]
    public async Task BulkUpsert_WithValidRates_ShouldInsertSuccessfully()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.20m, 1),
                new ExchangeRateItemDto("EUR", "JPY", 130.50m, 1),
                new ExchangeRateItemDto("EUR", "GBP", 0.85m, 1)
            });

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        var domainResult = new BulkExchangeRateResult(
            InsertedCount: 3,
            UpdatedCount: 0,
            SkippedCount: 0,
            ProcessedCount: 3,
            TotalInJson: 3,
            Status: "Success");

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(domainResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.RatesInserted.Should().Be(3);
        result.Value.RatesUpdated.Should().Be(0);
        result.Value.RatesUnchanged.Should().Be(0);
        result.Value.TotalProcessed.Should().Be(3);

        MockExchangeRateRepository.Verify(
            x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.Is<IEnumerable<BulkExchangeRateItem>>(items =>
                    items.Count() == 3 &&
                    items.Any(i => i.CurrencyCode == "USD" && i.Rate == 1.20m) &&
                    items.Any(i => i.CurrencyCode == "JPY" && i.Rate == 130.50m) &&
                    items.Any(i => i.CurrencyCode == "GBP" && i.Rate == 0.85m)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task BulkUpsert_WithExistingRates_ShouldUpdateSuccessfully()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.21m, 1), // Updated rate
                new ExchangeRateItemDto("EUR", "JPY", 131.00m, 1) // Updated rate
            });

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        var domainResult = new BulkExchangeRateResult(
            InsertedCount: 0,
            UpdatedCount: 2,
            SkippedCount: 0,
            ProcessedCount: 2,
            TotalInJson: 2,
            Status: "Success");

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(domainResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.RatesInserted.Should().Be(0);
        result.Value.RatesUpdated.Should().Be(2);
        result.Value.RatesUnchanged.Should().Be(0);
        result.Value.TotalProcessed.Should().Be(2);
    }

    [Fact]
    public async Task BulkUpsert_WithMixedInsertAndUpdate_ShouldProcessCorrectly()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.20m, 1), // Insert
                new ExchangeRateItemDto("EUR", "JPY", 130.50m, 1), // Update
                new ExchangeRateItemDto("EUR", "GBP", 0.85m, 1) // Unchanged
            });

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        var domainResult = new BulkExchangeRateResult(
            InsertedCount: 1,
            UpdatedCount: 1,
            SkippedCount: 1,
            ProcessedCount: 3,
            TotalInJson: 3,
            Status: "Success");

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(domainResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.RatesInserted.Should().Be(1);
        result.Value.RatesUpdated.Should().Be(1);
        result.Value.RatesUnchanged.Should().Be(1);
        result.Value.TotalProcessed.Should().Be(3);
    }

    [Fact]
    public async Task BulkUpsert_WhenProviderNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 999,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.20m, 1)
            });

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Provider with ID 999 not found");

        // Verify BulkUpsertAsync was never called
        MockExchangeRateRepository.Verify(
            x => x.BulkUpsertAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task BulkUpsert_WithEmptyRatesList_ShouldProcessCorrectly()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>());

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        var domainResult = new BulkExchangeRateResult(
            InsertedCount: 0,
            UpdatedCount: 0,
            SkippedCount: 0,
            ProcessedCount: 0,
            TotalInJson: 0,
            Status: "Success");

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(domainResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalProcessed.Should().Be(0);
    }

    [Fact]
    public async Task BulkUpsert_WhenRepositoryThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.20m, 1)
            });

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Error processing exchange rates");
        result.Error.Should().Contain("Database error");
    }

    [Fact]
    public async Task BulkUpsert_WithMultiplierValues_ShouldPassCorrectValues()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.20m, 1),
                new ExchangeRateItemDto("EUR", "JPY", 13050m, 100) // Rate with multiplier 100
            });

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        var domainResult = new BulkExchangeRateResult(
            InsertedCount: 2,
            UpdatedCount: 0,
            SkippedCount: 0,
            ProcessedCount: 2,
            TotalInJson: 2,
            Status: "Success");

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(domainResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify correct multiplier values were passed
        MockExchangeRateRepository.Verify(
            x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.Is<IEnumerable<BulkExchangeRateItem>>(items =>
                    items.Any(i => i.CurrencyCode == "USD" && i.Multiplier == 1) &&
                    items.Any(i => i.CurrencyCode == "JPY" && i.Multiplier == 100)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task BulkUpsert_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var command = new BulkUpsertExchangeRatesCommand(
            ProviderId: 1,
            ValidDate: new DateOnly(2025, 11, 6),
            Rates: new List<ExchangeRateItemDto>
            {
                new ExchangeRateItemDto("EUR", "USD", 1.20m, 1)
            });

        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, token))
            .ReturnsAsync(provider);

        var domainResult = new BulkExchangeRateResult(
            InsertedCount: 1,
            UpdatedCount: 0,
            SkippedCount: 0,
            ProcessedCount: 1,
            TotalInJson: 1,
            Status: "Success");

        MockExchangeRateRepository
            .Setup(x => x.BulkUpsertAsync(
                1,
                new DateOnly(2025, 11, 6),
                It.IsAny<IEnumerable<BulkExchangeRateItem>>(),
                token))
            .ReturnsAsync(domainResult);

        // Act
        var result = await _handler.Handle(command, token);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify token was passed through
        MockProviderRepository.Verify(
            x => x.GetByIdAsync(1, token),
            Times.Once);

        MockExchangeRateRepository.Verify(
            x => x.BulkUpsertAsync(1, new DateOnly(2025, 11, 6), It.IsAny<IEnumerable<BulkExchangeRateItem>>(), token),
            Times.Once);
    }
}
