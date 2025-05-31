using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Services.Models.CzechNationalBankApi;

namespace Services.Test;

public class CzechCrownRateProviderShould
{
    private readonly CzkExchangeRateResponse _defaultPrimaryRates =
        new()
        {
            Rates = [
                new RateResponse { Amount = 1, CurrencyCode = "USD", Rate = 21.5m, ValidFor = DateTime.UtcNow },
                new RateResponse { Amount = 1, CurrencyCode = "EUR", Rate = 30.0m, ValidFor = DateTime.UtcNow },
                new RateResponse { Amount = 5, CurrencyCode = "HKD", Rate = 40.0m, ValidFor = DateTime.UtcNow }
            ]
        };

    private readonly CzkExchangeRateResponse _defaultSecondaryRates =
        new()
        {
            Rates = [
                new RateResponse { Amount = 1, CurrencyCode = "AOA", Rate = 44.5m, ValidFor = DateTime.UtcNow },
                new RateResponse { Amount = 1, CurrencyCode = "AWG", Rate = 51.0m, ValidFor = DateTime.UtcNow },
                new RateResponse { Amount = 5, CurrencyCode = "BZD", Rate = 55.0m, ValidFor = DateTime.UtcNow },
            ]
        };

    [Fact]
    public async Task Should_CallOnlyExchangeRateEndpoint_IfResultIsFound()
    {
        var (provider, client, _) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("EUR")])).ToList();

        AssertExchangeRates(response, ("USD", 21.5m), ("EUR", 30.0m));

        A.CallTo(() => client.GetExchangeRatesAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).MustNotHaveHappened();
    }

    [Fact]
    public async Task Should_CallSecondaryEndpoint_IfResultIsNoFoundInPrimary()
    {
        var (provider, client, _) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("AOA")])).ToList();

        AssertExchangeRates(response, ("USD", 21.5m), ("AOA", 44.5m));

        A.CallTo(() => client.GetExchangeRatesAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_ReturnResultsAndIgnoreCurrencyRatesNotFound()
    {
        var (provider, client, _) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("TRY"), new Currency("BOB")])).ToList();

        AssertExchangeRates(response, ("USD", 21.5m));

        A.CallTo(() => client.GetExchangeRatesAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_UseCachedResultForPrimaryEndpoint()
    {
        var (provider, client, cache) = Setup(withCachedResult: true);

        var response = (await provider.GetExchangeRates([new Currency("EUR"), new Currency("USD")])).ToList();

        AssertExchangeRates(response, ("EUR", 70m), ("USD", 65m));

        A.CallTo(() => cache.GetStringAsync("EUR", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => cache.GetStringAsync("USD", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();

        A.CallTo(() => client.GetExchangeRatesAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).MustNotHaveHappened();
    }

    [Fact]
    public async Task Should_UseCachedResultForSecondaryEndpoint()
    {
        var (provider, client, cache) = Setup(withCachedResult: true);

        var response = (await provider.GetExchangeRates([new Currency("AWG"), new Currency("AOA")])).ToList();

        AssertExchangeRates(response, ("AOA", 110m), ("AWG", 90m));

        A.CallTo(() => cache.GetStringAsync("AWG", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => cache.GetStringAsync("AOA", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();

        A.CallTo(() => client.GetExchangeRatesAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_UseAmountFieldToCalculateRate()
    {
        var (provider, _, _) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("HKD")])).ToList();

        response.Should().HaveCount(1);
        response[0].Value.Should().Be(8m);
    }

    [Fact]
    public async Task Should_StoreResultsToCache()
    {
        var (provider, client, cache) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("AOA")])).ToList();

        response.Should().HaveCount(2);

        A.CallTo(() => client.GetExchangeRatesAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).MustNotHaveHappened();

        A.CallTo(() => cache.GetStringAsync("USD", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => cache.GetStringAsync("AOA", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => cache.SetStringAsync("USD", "21.5", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => cache.SetStringAsync("EUR", "30.0", A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_ReturnResultInAlphabeticalOrder()
    {
        var (provider, _, _) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("HKD"), new Currency("EUR"), new Currency("USD"), new Currency("AOA")])).ToList();

        response.Should().HaveCount(4);
        response[0].TargetCurrency.Code.Should().Be("AOA");
        response[1].TargetCurrency.Code.Should().Be("EUR");
        response[2].TargetCurrency.Code.Should().Be("HKD");
        response[3].TargetCurrency.Code.Should().Be("USD");
    }

    private (CzechCrownRateProvider provider, ICzechNationalBankClient client, IDistributedCache distributedCache) Setup(bool withCachedResult = false)
    {
        var client = A.Fake<ICzechNationalBankClient>();
        var distributedCache = A.Fake<IDistributedCache>();
        if (withCachedResult)
        {
            A.CallTo(() => distributedCache.GetStringAsync("USD", A<CancellationToken>.Ignored)).Returns("65.0");
            A.CallTo(() => distributedCache.GetStringAsync("EUR", A<CancellationToken>.Ignored)).Returns("70.0");
            A.CallTo(() => distributedCache.GetStringAsync("AOA", A<CancellationToken>.Ignored)).Returns("110.0");
            A.CallTo(() => distributedCache.GetStringAsync("AWG", A<CancellationToken>.Ignored)).Returns("90.0");
        }
        A.CallTo(() => client.GetExchangeRatesAsync()).Returns(_defaultPrimaryRates);
        A.CallTo(() => client.GetOtherExchangeRatesAsync()).Returns(_defaultSecondaryRates);
        var provider = new CzechCrownRateProvider(client, distributedCache);
        return (provider, client, distributedCache);
    }

    private static void AssertExchangeRates(IList<ExchangeRate> response, params (string currencyCode, decimal value)[] expectedRates)
    {
        response.Should().HaveCount(expectedRates.Length);

        response.Should().AllSatisfy(r =>
        {
            r.SourceCurrency.Code.Should().Be("CZK");
        });
        foreach (var rate in expectedRates)
        {
            response.Should().Contain(r => r.Value == rate.value && r.TargetCurrency.Code == rate.currencyCode);
        }
    }
}
