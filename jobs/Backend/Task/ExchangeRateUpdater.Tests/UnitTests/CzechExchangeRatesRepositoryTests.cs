using ExchangeRateUpdater.Repositories;
using ExchangeRateUpdater.Tests.UnitTests.Helpers;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests.UnitTests;

public class CzechExchangeRatesRepositoryTests
{

    [Fact]
    public async void Returns_happy_response()
    {
        var rateResources = CreateRates(
            ("NZD", 1, 2M),
            ("USD", 1, 4M));
        var httpClient = new HttpClient(new StubHttpMessageHandler(rateResources))
        {
            BaseAddress = new Uri("https://api.cnb.cz"),
        };
        var sut = new CzechExchangeRatesRepository(httpClient);

        var rates = await sut.GetExchangeRatesAsync();

        rates.Count.Should().Be(2);
        rates.Should()
            .ContainEquivalentOf(CreateExchangeRate("NZD", "CZK", 2M)).And
            .ContainEquivalentOf(CreateExchangeRate("USD", "CZK", 4M));
    }
    
    
    public static TheoryData<int, decimal, decimal> Data =>
        new()
        {
            { 10, 24M, 2.4M },
            { 200, 24M, 0.12M },
            { 0, 24M, 24M },
            { -19, 24M, 24M },
        };

    [Theory]
    [MemberData(nameof(Data))]
    public async void Calculates_the_rate_to_1_of_the_source_currency(int amount, decimal rate, decimal expectedRate)
    {
        var rateResources = CreateRates(("NZD", amount, rate));
        var httpClient = new HttpClient(new StubHttpMessageHandler(rateResources))
        {
            BaseAddress = new Uri("https://api.cnb.cz"),
        };
        var sut = new CzechExchangeRatesRepository(httpClient);

        var rates = await sut.GetExchangeRatesAsync();

        rates.Count().Should().Be(1);
        rates.First().Value.Should().Be(expectedRate);
    }
    
    private static ExchangeRate CreateExchangeRate(string sourceCurrency, string targetCurrency, decimal value)
    {
        return new ExchangeRate(new Currency(sourceCurrency), new Currency(targetCurrency), value);
    }

    private static List<RateResource> CreateRates(params (string currencyCode, int amount, decimal rate)[] inputs)
    {
        return inputs.Select(arg =>
        {
            var (currencyCode, amount, rate) = arg;
            return new RateResource()
            {
                Amount = amount,
                CurrencyCode = currencyCode,
                Rate = rate
            };
        }).ToList();
    }
}