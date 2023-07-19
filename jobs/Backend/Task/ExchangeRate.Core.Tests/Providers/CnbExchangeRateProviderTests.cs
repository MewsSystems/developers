using ExchangeRate.Core.Exceptions;
using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Models;
using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Providers;
using Moq;
using Xunit;

namespace ExchangeRate.Core.Tests.Providers;

public class CnbExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateSourceClient<CnbExchangeRateResponse>> _clientMock;

    public CnbExchangeRateProviderTests()
    {
        _clientMock = new Mock<IExchangeRateSourceClient<CnbExchangeRateResponse>>();
    }

    [Fact]
    public async void GetExchangeRatesAsync_CurrenciesAreNull_ThrowsArgumentNullException()
    {
        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await cnbExchangeRateProvider.GetExchangeRatesAsync(null));
    }

    [Fact]
    public async void GetExchangeRatesAsync_ClientReturnsError_ThrowsExchangeSourceException()
    {
        _clientMock
            .Setup(c => c.GetExchangeRatesAsync(It.IsAny<string>()))
            .ThrowsAsync(new ExchangeRateSourceException("Invalid request"));

        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object);

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

        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object);

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

        var existingCurrencies = inputCurrencies
            .Intersect(
                existingRates.Select(r => new Currency(r.Currency)))
            .ToList();

        var cnbExchangeRateProvider = new CnbExchangeRateProvider(_clientMock.Object);

        var exchangeRates = await cnbExchangeRateProvider.GetExchangeRatesAsync(inputCurrencies);

        Assert.True(
            existingCurrencies.All(
                c => exchangeRates.Any(
                    r => string.Equals(r.TargetCurrency.Code, c.Code, StringComparison.InvariantCultureIgnoreCase))));
    }

    private IEnumerable<CnbExchangeRateResponse> GetClientResponse()
    {
        return new List<CnbExchangeRateResponse>
        {
            new CnbExchangeRateResponse
            {
                Currency = "BGN",
                Rate = 1.2f
            },
            new CnbExchangeRateResponse
            {
                Currency = "CAD",
                Rate = 23.1f
            },
            new CnbExchangeRateResponse
            {
                Currency = "DKK",
                Rate = 4.2f
            },
            new CnbExchangeRateResponse
            {
                Currency = "ILS",
                Rate = 0.42f
            },
            new CnbExchangeRateResponse
            {
                Currency = "NZD",
                Rate = 42.4f
            },
            new CnbExchangeRateResponse
            {
                Currency = "SGD",
                Rate = 0.98f
            }
        };
    }
}