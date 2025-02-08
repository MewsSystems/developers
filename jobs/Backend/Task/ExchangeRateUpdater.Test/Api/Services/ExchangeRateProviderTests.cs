using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Contract;
using ExchangeRateUpdater.Contract.ExchangeRate;
using FuncSharp;
using NFluent;
using NSubstitute;

namespace ExchangeRateUpdater.Test.Api.Services;

public sealed class ExchangeRateProviderTests
{
    private ICnbExchangeRateFetcher _fetcher;
    private ExchangeRateProvider _provider;

    [SetUp]
    public void SetUp()
    {
        _fetcher = Substitute.For<ICnbExchangeRateFetcher>();
        _provider = new ExchangeRateProvider(_fetcher);
    }

    [Test]
    public async Task GetExchangeRatesAsync_NoCurrencies_ReturnsEmptyList()
    {
        var result = await _provider.GetExchangeRatesAsync(new List<Currency>(), CancellationToken.None);
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Success.Get()).IsEmpty();
    }

    [Test]
    public async Task GetExchangeRatesAsync_FetcherReturnsRates_FiltersAndMapsRates()
    {
        // Arrange
        var currencies = new List<Currency> { Currency.Usd, Currency.Eur, Currency.Php };
        var fetchedRates = new List<CnbExchangeRate>
        {
            new(Order: 94, Country: "USA", Currency: "dollar", CurrencyCode: Currency.Usd, Amount: 1, Rate: 23.048m),
            new(Order: 94, Country: "EMU", Currency: "euro", CurrencyCode: Currency.Eur, Amount: 1, Rate: 25.75m),
            new(Order: 94, Country: "Filipíny", Currency: "peso", CurrencyCode: Currency.Php, Amount: 100, Rate: 43.71m)
        };
        var cnbRates = Try.Success<IEnumerable<CnbExchangeRate>, CnbExchangeRatesFetchError>(fetchedRates);
        var rates = new List<ExchangeRate>
        {
            new(SourceCurrency: Currency.Usd, TargetCurrency: Currency.Czk, Value: 23.048m),
            new(SourceCurrency: Currency.Eur, TargetCurrency: Currency.Czk, Value: 25.75m),
            new(SourceCurrency: Currency.Php, TargetCurrency: Currency.Czk, Value: 0.4371m)
        };

        _fetcher.FetchExchangeRatesAsync(CancellationToken.None).Returns(cnbRates);

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

        // Assert
        Check.That(result.IsSuccess).IsTrue();
        Check.That(result.Success.Get()).ContainsExactly(rates);
    }

    [Test]
    public async Task GetExchangeRatesAsync_FetcherReturnsNoData_ReturnsDataIssuesError()
    {
        // Arrange
        var currencies = new List<Currency> { Currency.Usd };
        _fetcher.FetchExchangeRatesAsync(Arg.Any<CancellationToken>()).Returns(
            Try.Error<IEnumerable<CnbExchangeRate>, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.NoData));

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(GetExchangeRatesError.DataIssues);
    }

    [Test]
    public async Task GetExchangeRatesAsync_FetcherReturnsTimeout_ReturnsServiceUnavailable()
    {
        // Arrange
        var currencies = new List<Currency> { Currency.Usd };

        _fetcher.FetchExchangeRatesAsync(Arg.Any<CancellationToken>()).Returns(
            Try.Error<IEnumerable<CnbExchangeRate>, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.Timeout));

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(GetExchangeRatesError.ServiceUnavailable);
    }

    [Test]
    public async Task GetExchangeRatesAsync_FetcherReturnsUnknownError_ReturnsUnknownError()
    {
        // Arrange
        var currencies = new List<Currency> { Currency.Usd };

        _fetcher.FetchExchangeRatesAsync(Arg.Any<CancellationToken>()).Returns(
            Try.Error<IEnumerable<CnbExchangeRate>, CnbExchangeRatesFetchError>(CnbExchangeRatesFetchError.Unknown));

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

        // Assert
        Check.That(result.IsError).IsTrue();
        Check.That(result.Error.Get()).IsEqualTo(GetExchangeRatesError.Unknown);
    }
}