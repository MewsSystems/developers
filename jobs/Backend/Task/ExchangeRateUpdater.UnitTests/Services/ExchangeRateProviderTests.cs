using ExchangeRateUpdater.Application.Configuration;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.UnitTests.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.Services;

public sealed class ExchangeRateProviderTests
{
    private readonly ExchangeRateConfig _exchangeRateConfig = new()
    { 
        DefaultCurrency = "CZK"
    };
    
    private readonly ExchangeRateProvider _exchangeRateProvider;
    private readonly Mock<ILogger<ExchangeRateProvider>> _logger = new();

    public ExchangeRateProviderTests()
    {
        var mockApi = new Mock<IExchangeRateApiClient>();
        mockApi.Setup(x => x.GetAllExchangeRatesAsync(CancellationToken.None))
            .ReturnsAsync([
                new CnbExchangeRate("USD", 10, 1),
                new CnbExchangeRate("EUR", 20, 1),
                new CnbExchangeRate("GBP", 30, 10)
            ]);
        _exchangeRateProvider =
            new ExchangeRateProvider(mockApi.Object, Options.Create(_exchangeRateConfig), _logger.Object);
    }

    [Fact]
    public async Task GetExchangeRates_WithValidCurrencies_ReturnedOnlyProvidedCurrencyRate()
    {
        // Arrange
        string[] currencies = ["USD", "EUR"];

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(currencies, CancellationToken.None);

        // Assert
        var expected = new List<ExchangeRate>
        {
            new("USD", "CZK", 10),
            new("EUR", "CZK", 20),
        };
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetExchangeRates_WithInvalidCurrency_ReturnedOnlyValidCurrencyRate()
    {
        // Arrange
        const string nonExistingCurrency = "XXX";
        string[] currencies = ["USD", nonExistingCurrency];

        // Act
        var result = await _exchangeRateProvider.GetExchangeRates(currencies, CancellationToken.None);

        // Assert
        var expected = new List<ExchangeRate>
        {
            new("USD", "CZK", 10)
        };
        Assert.Equal(expected, result);
        _logger.VerifyLogged(LogLevel.Warning, "No matches were found among CNB rates");
    }
}