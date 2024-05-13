using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests;

public sealed class ExchangeRateProviderTests
{
    private IExchangeRateProvider _exchangeRateProvider;

    private readonly Mock<ICnbHttpClient> _cnbHttpClientMock = new();
    private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock = new();

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
    private const decimal JpyRate = 14;
    private const int JpyAmount = 100;

    public ExchangeRateProviderTests()
    {
        var options = GetOptions();

        _exchangeRateProvider = new ExchangeRateProvider(options, _cnbHttpClientMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void GetActualCurrencyCodes_SuccessfullyReceiveRequiredCurrencyCodes_WhenSettingsAreValid()
    {
        var actualCurrencyCodes = _exchangeRateProvider.GetActualCurrencyCodes();

        Assert.Equal(RequiredCurrencyCodes, actualCurrencyCodes);
    }

    [Fact]
    public void GetActualCurrencyCodes_SuccessfullyReceiveEmptyCollection_WhenSettingsAreEmpty()
    {
        _exchangeRateProvider = new ExchangeRateProvider(
            GetOptions(new HashSet<string>()),
            _cnbHttpClientMock.Object,
            _loggerMock.Object);

        var actualCurrencyCodes = _exchangeRateProvider.GetActualCurrencyCodes();

        Assert.Empty(actualCurrencyCodes);
    }

    [Fact]
    public async Task GetExchangeRates_SuccessfullyReceiveExchangeRates_WhenInputIsValid()
    {
        var requiredCurrencyCodes = new HashSet<string>
        {
            "USD",
            "EUR",
            "ERR"
        };

        IReadOnlyCollection<CnbExchangeRate> cnbExchangeRates = new List<CnbExchangeRate>
        {
            new(1, "USD", UsdRate),
            new(1, "EUR", EurRate)
        };

        var expectedExchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), UsdRate),
            new(new Currency("EUR"), new Currency("CZK"), EurRate)
        };

        _cnbHttpClientMock.Setup(x => x.GetDailyExchangeRates(CancellationToken.None))
            .Returns(Task.FromResult(cnbExchangeRates));

        var actualExchangeRates =
            await _exchangeRateProvider.GetExchangeRates(requiredCurrencyCodes, CancellationToken.None);

        Assert.Equal(expectedExchangeRates, actualExchangeRates);
        Assert.Collection(
            actualExchangeRates,
            x => Assert.Contains("USD", x.SourceCurrency.Code),
            x => Assert.Contains("EUR", x.SourceCurrency.Code));
    }

    [Fact]
    public async Task GetExchangeRates_SuccessfullyReceiveEmptyCollection_WhenCnbDoesNotProvideCurrencies()
    {
        var requiredCurrencyCodes = new HashSet<string>
        {
            "VBN",
            "ZXC",
            "ERR"
        };

        IReadOnlyCollection<CnbExchangeRate> cnbExchangeRates = new List<CnbExchangeRate>
        {
            new(1, "USD", UsdRate),
            new(1, "EUR", EurRate)
        };

        _cnbHttpClientMock.Setup(x => x.GetDailyExchangeRates(CancellationToken.None))
            .Returns(Task.FromResult(cnbExchangeRates));

        var actualExchangeRates =
            await _exchangeRateProvider.GetExchangeRates(requiredCurrencyCodes, CancellationToken.None);

        Assert.Empty(actualExchangeRates);
    }

    [Fact]
    public async Task GetExchangeRates_SuccessfullyReceiveRatePerUnit_WhenRateContainsAmountGreaterThenOne()
    {
        var requiredCurrencyCodes = new HashSet<string>
        {
            "JPY"
        };

        var rateWithAmount = new CnbExchangeRate(JpyAmount, "JPY", JpyRate);
        IReadOnlyCollection<CnbExchangeRate> cnbExchangeRates = new List<CnbExchangeRate>
        {
            rateWithAmount
        };

        _cnbHttpClientMock.Setup(x => x.GetDailyExchangeRates(CancellationToken.None))
            .Returns(Task.FromResult(cnbExchangeRates));

        var actualExchangeRates =
            await _exchangeRateProvider.GetExchangeRates(requiredCurrencyCodes, CancellationToken.None);

        Assert.Equal(rateWithAmount.Rate / rateWithAmount.Amount, actualExchangeRates[0].Value);
    }

    [Fact]
    public async Task GetExchangeRates_FailedReceiveExchangeRates_WhenHttpClientThrowsError()
    {
        var expectedException = new HttpRequestException("400/404/500");
        _cnbHttpClientMock.Setup(x => x.GetDailyExchangeRates(CancellationToken.None))
            .Throws(expectedException);

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
            await _exchangeRateProvider.GetExchangeRates(RequiredCurrencyCodes, CancellationToken.None)
        );
    }

    private static ExchangeOptions GetOptions(HashSet<string>? requiredCurrencyCodes = null)
    {
        return new ExchangeOptions
        {
            CnbRatesUrl = "https://api.test/",
            TargetCurrencyCode = "CZK",
            RequiredCurrencyCodes = requiredCurrencyCodes ?? RequiredCurrencyCodes
        };
    }
}