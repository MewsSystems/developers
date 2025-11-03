using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.RateExporters;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Services.RateExporters;

public class ConsoleExchangeRateExporterTests : IDisposable
{
    private readonly StringWriter _consoleOutput;
    private readonly ConsoleExchangeRateExporter _exporter;
    private readonly Mock<ILogger<ConsoleExchangeRateExporter>> _mockLogger;
    private readonly TextWriter _originalConsoleOut;

    public ConsoleExchangeRateExporterTests()
    {
        _mockLogger = new Mock<ILogger<ConsoleExchangeRateExporter>>();
        _exporter = new ConsoleExchangeRateExporter(_mockLogger.Object);

        _originalConsoleOut = Console.Out;
        _consoleOutput = new StringWriter();
        Console.SetOut(_consoleOutput);
    }

    public void Dispose()
    {
        Console.SetOut(_originalConsoleOut);
        _consoleOutput.Dispose();
    }

    [Fact]
    public void Constructor_WithValidLogger_CreatesInstance()
    {
        // Arrange & Act
        var exporter = new ConsoleExchangeRateExporter(_mockLogger.Object);

        // Assert
        Assert.NotNull(exporter);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithEmptyList_LogsZeroRates()
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
    public async Task ExportExchangeRatesAsync_WithEmptyList_WritesNothingToConsole()
    {
        // Arrange
        var emptyRates = Enumerable.Empty<ExchangeRate>();

        // Act
        await _exporter.ExportExchangeRatesAsync(emptyRates);

        // Assert
        var output = _consoleOutput.ToString();
        Assert.Empty(output.Trim());
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithSingleRate_LogsOneRate()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(
                new Currency("USD"),
                new Currency("CZK"),
                DateOnly.MaxValue,
                22.5m)
        };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("1 exchange rates")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_LogsCorrectCount()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, 22.5m),
            new(new Currency("EUR"), new Currency("CZK"), DateOnly.MaxValue, 25.0m),
            new(new Currency("GBP"), new Currency("CZK"), DateOnly.MaxValue, 30.0m)
        };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

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
    public async Task ExportExchangeRatesAsync_WithMultipleRates_WritesAllRatesToConsole()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, 22.5m),
            new(new Currency("EUR"), new Currency("CZK"), DateOnly.MaxValue, 25.0m),
            new(new Currency("GBP"), new Currency("CZK"), DateOnly.MaxValue, 30.0m)
        };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        var output = _consoleOutput.ToString();
        Assert.Contains("USD", output);
        Assert.Contains("EUR", output);
        Assert.Contains("GBP", output);
        Assert.Contains("22", output);
        Assert.Contains("25", output);
        Assert.Contains("30", output);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_WithMultipleRates_WritesEachRateOnSeparateLine()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, 22.5m),
            new(new Currency("EUR"), new Currency("CZK"), DateOnly.MaxValue, 25.0m)
        };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        var output = _consoleOutput.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.True(lines.Length >= 2);
    }

    [Fact]
    public async Task ExportExchangeRatesAsync_CallsLoggerExactlyOnce()
    {
        // Arrange
        var rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), DateOnly.MaxValue, 22.5m)
        };

        // Act
        await _exporter.ExportExchangeRatesAsync(rates);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}