using ExchangeRateUpdater.CNBRateProvider.Client;
using ExchangeRateUpdater.Domain.Models;
using FluentResults;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.CNBRateProvider.UnitTests;

public class ExchangeRateProviderShould
{
    private readonly Mock<ICnbClient> _clientMock = new();

    private static readonly CurrencyPair UsdToCzkCurrencyPair = new(Currency.FromString("USD"), Currency.FromString("CZK"));
    private readonly IEnumerable<CurrencyPair> _currencyPairs =
        new List<CurrencyPair>
        {
            UsdToCzkCurrencyPair
        };

    [Fact]
    public async Task ReturnErrorIfFailedToObtainRates()
    {
        // arrange
        _clientMock
            .Setup(c => c.GetDailyExchangeRateToCzk(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("..."));

        var provider = new ExchangeRateProvider(_clientMock.Object);

        // act
        var providerResult = await provider.GetExchangeRates(_currencyPairs, CancellationToken.None);

        // assert
        Assert.True(providerResult.IsFailed);
    }

    [Fact]
    public async Task ReturnOnlyRequestedCurrencies()
    {
        // arrange
        IEnumerable<ExchangeRate> exchangeRates = new List<ExchangeRate>
        {
            new(UsdToCzkCurrencyPair, 1.01m),
            new(new CurrencyPair(Currency.FromString("CZK"), Currency.FromString("USD")), 1.01m),
        };
        _clientMock
            .Setup(c => c.GetDailyExchangeRateToCzk(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(exchangeRates));

        var provider = new ExchangeRateProvider(_clientMock.Object);

        // act
        var providerResult = await provider.GetExchangeRates(_currencyPairs, CancellationToken.None);

        // assert
        Assert.False(providerResult.IsFailed);

        var receivedExchangeRates = providerResult.Value;
        Assert.Equal(1, receivedExchangeRates.Count);
        Assert.Equal(UsdToCzkCurrencyPair, receivedExchangeRates.First().CurrencyPair);
        Assert.Equal(1.01m, receivedExchangeRates.First().Value);
    }

    [Theory]
    [MemberData(nameof(GenerateNonUsdToCzkCurrencyRates))]
    public async Task ReturnEmptyIfNoCurrencyPresent(IEnumerable<ExchangeRate> exchangeRates)
    {
        // arrange
        _clientMock
            .Setup(c => c.GetDailyExchangeRateToCzk(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(exchangeRates));

        var provider = new ExchangeRateProvider(_clientMock.Object);

        // act
        var providerResult = await provider.GetExchangeRates(_currencyPairs, CancellationToken.None);

        // assert
        Assert.False(providerResult.IsFailed);
        Assert.Equal(0, providerResult.Value.Count);
    }

    [Fact]
    public async Task ReturnEmptyIfSourceEmpty()
    {
        // arrange
        _clientMock
            .Setup(c => c.GetDailyExchangeRateToCzk(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok((IEnumerable<ExchangeRate>)Array.Empty<ExchangeRate>()));

        var provider = new ExchangeRateProvider(_clientMock.Object);

        // act
        var providerResult = await provider.GetExchangeRates(_currencyPairs, CancellationToken.None);

        // assert
        Assert.False(providerResult.IsFailed);
        Assert.Equal(0, providerResult.Value.Count);
    }

    public static IEnumerable<object[]> GenerateNonUsdToCzkCurrencyRates()
    {
        yield return new object[]
        {
            new List<ExchangeRate>
            {
                new(new CurrencyPair(Currency.FromString("CZK"), Currency.FromString("USD")), 1.01m)
            }
        };
        yield return new object[]
        {
            new List<ExchangeRate>
            {
                new(new CurrencyPair(Currency.FromString("USD"), Currency.FromString("NoCZK")), 1.01m)
            }
        };
    }
}
