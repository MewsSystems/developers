using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests;

public sealed class HostedProcessingServiceTests
{
    private readonly IHostedService _hostedService;
    private readonly Mock<IExchangeRateProvider> _exchangeRateProviderMock = new();
    private readonly Mock<IConsoleManager> _consoleManagerMock = new();
    private readonly Mock<ILogger<HostedProcessingService>> _loggerMock = new();

    private static readonly HashSet<string> RequiredCurrencyCodes = new()
    {
        "USD",
        "EUR",
        "CZK",
        "JPY",
        "KES",
        "RUB",
        "THB",
        "TRY",
        "XYZ"
    };
    
    private const decimal UsdRate = 23;
    private const decimal EurRate = 25;
    
    public HostedProcessingServiceTests()
    {
        _exchangeRateProviderMock.Setup(x => x.GetActualCurrencyCodes())
            .Returns(RequiredCurrencyCodes);
        _consoleManagerMock.Setup(x => x.ReadLine()).Returns(string.Empty);

        _hostedService = new HostedProcessingService(_exchangeRateProviderMock.Object, _consoleManagerMock.Object, _loggerMock.Object);
    }
    
    [Fact]
    public async Task Process_SuccessfullyReceiveExchangeRates_WhenInputIsValid()
    {
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), UsdRate),
            new(new Currency("EUR"), new Currency("CZK"), EurRate)
        };

        _exchangeRateProviderMock
            .Setup(x => x.GetExchangeRates(
                It.IsAny<HashSet<string>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(exchangeRates));

        await _hostedService.StartAsync(CancellationToken.None);
        
        _exchangeRateProviderMock.Verify(x => x.GetActualCurrencyCodes(), Times.Once);
        
        _consoleManagerMock.Verify(x => x.WriteLine($"Successfully retrieved {exchangeRates.Count} exchange rates:"),
            Times.Once);
        _consoleManagerMock.Verify(x => x.WriteLine(exchangeRates[0].ToString()), Times.Once);
        _consoleManagerMock.Verify(x => x.WriteLine(exchangeRates[1].ToString()), Times.Once);

        _exchangeRateProviderMock.Verify(
            x => x.GetExchangeRates(RequiredCurrencyCodes, CancellationToken.None), Times.Once);

        _consoleManagerMock.Verify(x => x.ReadLine(), Times.Once);
    }

    [Fact]
    public async Task Process_HandleException_WhenSomethingWrong()
    {
        var expectedException = new HttpRequestException("404");
        _exchangeRateProviderMock
            .Setup(x => x.GetExchangeRates(
                It.IsAny<HashSet<string>>(),
                It.IsAny<CancellationToken>()))
            .Throws(expectedException);

        await _hostedService.StartAsync(CancellationToken.None);

        _consoleManagerMock.Verify(
            x => x.WriteLine($"Could not retrieve exchange rates: '{expectedException.Message}'."),
            Times.Once());
        
        _consoleManagerMock.Verify(x => x.ReadLine(), Times.Once);
    }
}