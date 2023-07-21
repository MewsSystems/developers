using ExchangeRate.Core.Configuration;
using ExchangeRate.Core.Exceptions;
using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Models;
using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Providers;
using ExchangeRate.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRate.Core.Tests.Providers;

public class CnbExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateSourceClient<CnbExchangeRate>> _clientMock;

    private readonly Mock<ICacheService> _cacheServiceMock;

    private readonly Mock<IOptions<CnbSettings>> _cnbSettingMock;

    public CnbExchangeRateProviderTests()
    {
        _clientMock = new Mock<IExchangeRateSourceClient<CnbExchangeRate>>();
        _cacheServiceMock = new Mock<ICacheService>();
        _cnbSettingMock = new Mock<IOptions<CnbSettings>>();

        _cnbSettingMock
            .SetupGet(s => s.Value)
            .Returns(new CnbSettings());
    }

    [Fact]
    public async void GetExchangeRatesAsync_CurrenciesAreNull_ThrowsArgumentNullException()
    {
        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object, _cacheServiceMock.Object, _cnbSettingMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await cnbExchangeRateProvider.GetExchangeRatesAsync(null));
    }

    [Fact]
    public async void GetExchangeRatesAsync_ClientReturnsError_ThrowsExchangeSourceException()
    {
        _clientMock
            .Setup(c => c.GetExchangeRatesAsync(It.IsAny<string>()))
            .ThrowsAsync(new ExchangeRateSourceException("Invalid request"));
        _cacheServiceMock
            .Setup(cs => cs.GetAsync<IEnumerable<CnbExchangeRate>>(It.IsAny<string>()))
            .ReturnsAsync(default(IEnumerable<CnbExchangeRate>));

        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object, _cacheServiceMock.Object, _cnbSettingMock.Object);

        await Assert.ThrowsAsync<ExchangeRateSourceException>(async () => await cnbExchangeRateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>()));
    }

    [Fact]
    public async void GetExchangeRatesAsync_ClientDoesNotReturnAnyOfSearchedCurrencies_ReturnsEmptyCollection()
    {
        var inputCurrencies = new List<Currency>
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("BRL")
        };

        _clientMock
            .Setup(c => c.GetExchangeRatesAsync(It.IsAny<string>()))
            .ReturnsAsync(GetClientResponse);

        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object, _cacheServiceMock.Object, _cnbSettingMock.Object);

        var exchangeRates = await cnbExchangeRateProvider.GetExchangeRatesAsync(inputCurrencies);

        Assert.Empty(exchangeRates);
    }

    [Theory]
    [InlineData("USD", "EUR", "BRL")]
    [InlineData("USD", "BGN", "EUR", "BRL", "DKK")]
    [InlineData("USD", "SGD", "BGN", "EUR", "BRL", "DKK", "NZD")]
    public async void GetExchangeRatesAsync_ClientReturnsSearchedCurrencies_ReturnsFoundExchangeRates(params string[] currencies)
    {
        var inputCurrencies = currencies
            .Select(c => new Currency(c))
            .ToList();

        var existingRates = GetClientResponse();

        _clientMock
            .Setup(c => c.GetExchangeRatesAsync(It.IsAny<string>()))
            .ReturnsAsync(existingRates);
        _cacheServiceMock
            .Setup(cs => cs.GetAsync<IEnumerable<CnbExchangeRate>>(It.IsAny<string>()))
            .ReturnsAsync(default(IEnumerable<CnbExchangeRate>));

        var existingCurrencies = inputCurrencies
            .Intersect(
                existingRates.Select(r => new Currency(r.Currency)));

        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object, _cacheServiceMock.Object, _cnbSettingMock.Object);

        var exchangeRates = (await cnbExchangeRateProvider.GetExchangeRatesAsync(inputCurrencies)).ToList();

        Assert.True(
            existingCurrencies.All(
                c => exchangeRates.Any(
                    r => string.Equals(r.TargetCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase))));
    }

    private IEnumerable<CnbExchangeRate> GetClientResponse()
    {
        return new List<CnbExchangeRate>
        {
            new CnbExchangeRate
            {
                CurrencyCode = "BGN",
                Rate = 1.2f,
                Amount = 1
            },
            new CnbExchangeRate
            {
                CurrencyCode = "CAD",
                Rate = 23.1f,
                Amount = 1
            },
            new CnbExchangeRate
            {
                CurrencyCode = "DKK",
                Rate = 4.2f,
                Amount = 1
            },
            new CnbExchangeRate
            {
                CurrencyCode = "ILS",
                Rate = 0.42f,
                Amount = 1
            },
            new CnbExchangeRate
            {
                CurrencyCode = "NZD",
                Rate = 42.4f,
                Amount = 10
            },
            new CnbExchangeRate
            {
                CurrencyCode = "SGD",
                Rate = 0.98f,
                Amount = 1
            }
        };
    }
}