using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.RateExporters;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Services.RateExporters;

public class DatabaseExchangeRateExporterTests
{
    private readonly DatabaseExchangeRateExporter _exporter;
    private readonly Mock<ILogger<DatabaseExchangeRateExporter>> _mockLogger;
    private readonly Mock<IRepository<ExchangeRateEntity>> _mockRepository;
    private readonly List<ExchangeRate> _multipleRates;
    private readonly ExchangeRate _singleRate;

    public DatabaseExchangeRateExporterTests()
    {
        _mockRepository = new Mock<IRepository<ExchangeRateEntity>>();
        _mockLogger = new Mock<ILogger<DatabaseExchangeRateExporter>>();
        _exporter = new DatabaseExchangeRateExporter(_mockRepository.Object, _mockLogger.Object);

        _singleRate = new ExchangeRate(
            new Currency("USD"),
            new Currency("CZK"),
            DateOnly.MaxValue,
            22.5m);

        _multipleRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, 22.5m),
            new(new Currency("EUR"), new Currency("CZK"), DateOnly.MaxValue, 25.0m),
            new(new Currency("GBP"), new Currency("CZK"), DateOnly.MaxValue, 30.0m)
        };
    }

    [Fact]
    public void Constructor_WithValidDependencies_CreatesInstance()
    {
        // Arrange & Act
        var exporter = new DatabaseExchangeRateExporter(_mockRepository.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(exporter);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithEmptyList_CallsSaveChangesOnce()
    {
        // Arrange
        var emptyRates = Enumerable.Empty<ExchangeRate>();

        // Act
        await _exporter.ExportExchangeRatesAsync(emptyRates);

        // Assert
        _mockRepository.Verify(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithEmptyList_LogsZeroCount()
    {
        // Arrange
        var emptyRates = Enumerable.Empty<ExchangeRate>();

        // Act
        await _exporter.ExportExchangeRatesAsync(emptyRates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("0 exchange rates")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithSingleRate_CallsAddOnce()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        _mockRepository.Verify(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()), Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithSingleRate_AddsCorrectEntity()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };
        ExchangeRateEntity capturedEntity = null;
        _mockRepository.Setup(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()))
            .Callback<ExchangeRateEntity>(entity => capturedEntity = entity);

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.Equal("USD", capturedEntity.SourceCurrency);
        Assert.Equal("CZK", capturedEntity.TargetCurrency);
        Assert.Equal(22.5m, capturedEntity.Rate);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithSingleRate_LogsAddedRate()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Added exchange rate")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithSingleRate_CallsSaveChangesOnce()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithSingleRate_LogsSuccessfulExport()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("Successfully exported") && v.ToString().Contains("1 exchange rates")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_CallsAddCorrectNumberOfTimes()
    {
        // Arrange
        // Act
        await _exporter.ExportExchangeRatesAsync(_multipleRates);

        // Assert
        _mockRepository.Verify(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()), Times.Exactly(3));
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_AddsAllEntities()
    {
        // Arrange
        var capturedEntities = new List<ExchangeRateEntity>();
        _mockRepository.Setup(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()))
            .Callback<ExchangeRateEntity>(entity => capturedEntities.Add(entity));

        // Act
        await _exporter.ExportExchangeRatesAsync(_multipleRates);

        // Assert
        Assert.Equal(3, capturedEntities.Count);
        Assert.Contains(capturedEntities, e => e.SourceCurrency == "USD" && e.Rate == 22.5m);
        Assert.Contains(capturedEntities, e => e.SourceCurrency == "EUR" && e.Rate == 25.0m);
        Assert.Contains(capturedEntities, e => e.SourceCurrency == "GBP" && e.Rate == 30.0m);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_LogsEachAddedRate()
    {
        // Arrange
        // Act
        await _exporter.ExportExchangeRatesAsync(_multipleRates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Added exchange rate")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Exactly(3));
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_LogsCorrectCount()
    {
        // Arrange
        // Act
        await _exporter.ExportExchangeRatesAsync(_multipleRates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("3 exchange rates")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_SavesAfterAllAdds()
    {
        // Arrange
        var sequence = new List<string>();
        _mockRepository.Setup(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()))
            .Callback(() => sequence.Add("Add"));
        _mockRepository.Setup(r => r.SaveChangesAsync())
            .Callback(() => sequence.Add("Save"));

        // Act
        await _exporter.ExportExchangeRatesAsync(_multipleRates);

        // Assert
        Assert.Equal(4, sequence.Count);
        Assert.Equal("Add", sequence[0]);
        Assert.Equal("Add", sequence[1]);
        Assert.Equal("Add", sequence[2]);
        Assert.Equal("Save", sequence[3]);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithZeroRate_AddsEntityWithZeroValue()
    {
        // Arrange
        var rateWithZero = new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, 0m);
        var rates = new List<ExchangeRate> { rateWithZero };
        ExchangeRateEntity capturedEntity = null;
        _mockRepository.Setup(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()))
            .Callback<ExchangeRateEntity>(entity => capturedEntity = entity);

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.Equal(0m, capturedEntity.Rate);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithNegativeRate_AddsEntityWithNegativeValue()
    {
        // Arrange
        var rateWithNegative = new ExchangeRate(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, -5.0m);
        var rates = new List<ExchangeRate> { rateWithNegative };
        ExchangeRateEntity capturedEntity = null;
        _mockRepository.Setup(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()))
            .Callback<ExchangeRateEntity>(entity => capturedEntity = entity);

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.Equal(-5.0m, capturedEntity.Rate);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WhenRepositoryThrows_PropagatesException()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };
        _mockRepository.Setup(r => r.AddExchangeRateAsync(It.IsAny<ExchangeRateEntity>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _exporter.ExportExchangeRatesAsync(rates));
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WhenSaveChangesThrows_PropagatesException()
    {
        // Arrange
        var rates = new List<ExchangeRate> { _singleRate };
        _mockRepository.Setup(r => r.SaveChangesAsync())
            .ThrowsAsync(new InvalidOperationException("Save failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _exporter.ExportExchangeRatesAsync(rates));
    }
}