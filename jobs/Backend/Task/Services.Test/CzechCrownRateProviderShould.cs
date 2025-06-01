using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Services.Models.CzechNationalBankApi;

namespace Services.Test;

public class CzechCrownRateProviderShould
{
    private readonly CzkExchangeRateResponse _defaultPrimaryRates =
        new()
        {
            Rates = [
                new RateResponse { Amount = 1, CurrencyCode = "USD", Rate = 21.5m, ValidFor = DateOnly.FromDateTime(DateTime.UtcNow) },
                new RateResponse { Amount = 1, CurrencyCode = "EUR", Rate = 30.0m, ValidFor = DateOnly.FromDateTime(DateTime.UtcNow) },
                new RateResponse { Amount = 5, CurrencyCode = "HKD", Rate = 40.0m, ValidFor = DateOnly.FromDateTime(DateTime.UtcNow) }
            ]
        };

    private readonly CzkExchangeRateResponse _defaultSecondaryRates =
        new()
        {
            Rates = [
                new RateResponse { Amount = 1, CurrencyCode = "AOA", Rate = 44.5m, ValidFor = DateOnly.FromDateTime(DateTime.UtcNow) },
                new RateResponse { Amount = 1, CurrencyCode = "AWG", Rate = 51.0m, ValidFor = DateOnly.FromDateTime(DateTime.UtcNow) },
                new RateResponse { Amount = 5, CurrencyCode = "BZD", Rate = 55.0m, ValidFor = DateOnly.FromDateTime(DateTime.UtcNow) },
            ]
        };

    [Fact]
    public async Task Should_CallOnlyExchangeRateEndpoint_IfResultIsFound()
    {
        var (provider, client) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("EUR")])).ToList();

        AssertExchangeRates(response, ("USD", 21.5m), ("EUR", 30.0m));

        A.CallTo(() => client.GetExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task Should_CallSecondaryEndpoint_IfResultIsNoFoundInPrimary()
    {
        var (provider, client) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("AOA")])).ToList();

        AssertExchangeRates(response, ("USD", 21.5m), ("AOA", 44.5m));

        A.CallTo(() => client.GetExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_ReturnResultsAndIgnoreCurrencyRatesNotFound()
    {
        var (provider, client) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("USD"), new Currency("TRY"), new Currency("BOB")])).ToList();

        AssertExchangeRates(response, ("USD", 21.5m));

        A.CallTo(() => client.GetExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => client.GetOtherExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Should_UseAmountFieldToCalculateRate()
    {
        var (provider, _) = Setup();

        var response = (await provider.GetExchangeRates([new Currency("HKD")])).ToList();

        response.Should().HaveCount(1);
        response[0].Value.Should().Be(8m);
    }


    [Theory]
    [InlineData("EUR", "USD", "HKD")]
    [InlineData("USD", "EUR")]
    [InlineData("BZD", "AOA")]
    [InlineData("AOA", "BZD")]
    [InlineData("EUR", "USD", "HKD", "BZD", "AWG", "AOA")]
    public async Task Should_ReturnResultInAlphabeticalOrder(params string[] currencies)
    {
        var (provider, _) = Setup();

        var parameters = currencies.Select(c => new Currency(c)).ToList();
        var response = (await provider.GetExchangeRates(parameters)).ToList();

        response.Should().HaveCount(currencies.Length);
        response.Should().BeInAscendingOrder(r => r.SourceCurrency.Code);
    }

    private (CzechCrownRateProvider provider, ICzechNationalBankClient client) Setup()
    {
        var client = A.Fake<ICzechNationalBankClient>();

        A.CallTo(() => client.GetExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).Returns(_defaultPrimaryRates);
        A.CallTo(() => client.GetOtherExchangeRates(A<DateOnly>.Ignored, A<CancellationToken>.Ignored)).Returns(_defaultSecondaryRates);
        var provider = new CzechCrownRateProvider(client, NullLogger<CzechCrownRateProvider>.Instance);
        return (provider, client);
    }

    private static void AssertExchangeRates(IList<ExchangeRate> response, params (string currencyCode, decimal value)[] expectedRates)
    {
        response.Should().HaveCount(expectedRates.Length);

        response.Should().AllSatisfy(r =>
        {
            r.TargetCurrency.Code.Should().Be("CZK");
        });
        foreach (var (currencyCode, value) in expectedRates)
        {
            response.Should().Contain(r => r.Value == value && r.SourceCurrency.Code == currencyCode);
        }
    }
}
