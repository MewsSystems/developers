using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.Application;

[TestFixture]
public class ExchangeRateUpdaterServiceTests
{
    private Mock<IExchangeRateProvider> _providerMock;
    private Mock<ILogger<ExchangeRateUpdaterService>> _loggerMock;
    private ExchangeRateUpdaterService _service;
    private List<Currency> _currencies;

    [SetUp]
    public void SetUp()
    {
        _providerMock = new Mock<IExchangeRateProvider>();
        _loggerMock = new Mock<ILogger<ExchangeRateUpdaterService>>();
        _currencies = new List<Currency>
        {
            new Currency("USD"),
            new Currency("EUR")
        };

        _service = new ExchangeRateUpdaterService(_providerMock.Object, 
            Options.Create(
                new CurrencyOptions { Currencies = new[] { "USD", "EUR" } }
            ),
            _loggerMock.Object);
    }

    [Test]
    public async Task RunAsync_WhenProviderReturnsRates_LogsSuccessfulRetrieval()
    {
        var exchangeRates = new List<ExchangeRate>
        {
            new ExchangeRate(new Currency("CZK"), new Currency("USD"), 22.0m),
            new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 25.0m)
        };

        _providerMock
            .Setup(p => p.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()))
            .ReturnsAsync(exchangeRates);

        
        await _service.RunAsync();

        _providerMock.Verify(p => p.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()), Times.Once);
        
        _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successfully retrieved 2 exchange rates:")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public async Task RunAsync_WhenProviderThrowsException_LogsError()
    {
        _providerMock
            .Setup(p => p.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()))
            .ThrowsAsync(new System.Exception("Network error"));

        await _service.RunAsync();
        
        _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Could not retrieve exchange rates")),
                It.IsAny<System.Exception>(),
                It.IsAny<Func<It.IsAnyType, System.Exception, string>>()),
            Times.Once);
        
    }
}