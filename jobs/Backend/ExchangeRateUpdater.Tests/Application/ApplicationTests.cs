using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.RateExporters;
using ExchangeRateUpdater.Services.RateProviders;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests.Application;

public class ApplicationTests
{
    private readonly Mock<ILogger<ExchangeRateUpdater.Application.Application>> _loggerMock;
    private readonly Mock<IAppConfiguration> _appConfigurationMock;
    private readonly Mock<IExchangeRateProvider> _rateProviderMock;
    private readonly Mock<IExchangeRateExporter> _rateExporterMock;
    private readonly ExchangeRateUpdater.Application.Application _sut;
    
    private readonly List<Currency> _currencies;
    private readonly List<ExchangeRate> _rates;


    public ApplicationTests()
    {
        _loggerMock = new Mock<ILogger<ExchangeRateUpdater.Application.Application>>();
        _appConfigurationMock = new Mock<IAppConfiguration>();
        _rateProviderMock = new Mock<IExchangeRateProvider>();
        _rateExporterMock = new Mock<IExchangeRateExporter>();

        _currencies = new List<Currency> { new("USD"), new("EUR") };
        _rates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), DateOnly.MinValue, 23.5m),
            new(new Currency("EUR"), new Currency("CZK"), DateOnly.MinValue, 25.0m)
        };
        
        _sut = new ExchangeRateUpdater.Application.Application(
            _loggerMock.Object,
            _appConfigurationMock.Object,
            _rateProviderMock.Object,
            _rateExporterMock.Object);
    }

    [Fact]
    public async Task RunAsync_WhenSuccessful_RetrievesAndExportsRates()
    {
        _appConfigurationMock.Setup(x => x.GetCurrencies()).Returns(_currencies);
        _rateProviderMock.Setup(x => x.GetExchangeRatesAsync(_currencies)).ReturnsAsync(_rates);

        // Act
        await _sut.RunAsync();

        // Assert
        _rateProviderMock.Verify(x => x.GetExchangeRatesAsync(_currencies), Times.Once);
        _rateExporterMock.Verify(x => x.ExportExchangeRatesAsync(_rates), Times.Once);
    }

    [Fact]
    public async Task RunAsync_WhenProviderThrowsException_LogsError()
    {
        // Arrange
        var exception = new Exception("Provider error");

        _appConfigurationMock.Setup(x => x.GetCurrencies()).Returns(_currencies);
        _rateProviderMock.Setup(x => x.GetExchangeRatesAsync(_currencies)).ThrowsAsync(exception);

        // Act
        await _sut.RunAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _rateExporterMock.Verify(x => x.ExportExchangeRatesAsync(It.IsAny<IEnumerable<ExchangeRate>>()), Times.Never);
    }

    [Fact]
    public async Task RunAsync_WhenExporterThrowsException_LogsError()
    {
        // Arrange
        var exception = new Exception("Exporter error");

        _appConfigurationMock.Setup(x => x.GetCurrencies()).Returns(_currencies);
        _rateProviderMock.Setup(x => x.GetExchangeRatesAsync(_currencies)).ReturnsAsync(_rates);
        _rateExporterMock.Setup(x => x.ExportExchangeRatesAsync(_rates)).ThrowsAsync(exception);

        // Act
        await _sut.RunAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}