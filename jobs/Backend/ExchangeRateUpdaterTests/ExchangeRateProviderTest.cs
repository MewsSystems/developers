using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateProviderTest
{
    [Test]
    [Category("E2E")]
    public async Task GetExchangeRatesAsync()
    {
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };
    
        var provider = new ExchangeRateProvider();
        var rates = await provider.GetExchangeRatesAsync(currencies).ToList();

        rates.ShouldBe(new[]
        {
            ("EUR", "CZK", 24.535M),
            ("JPY", "CZK", 0.18781M),
            ("RUB", "CZK", 0.28425M),
            ("THB", "CZK", 0.66716M),
            ("TRY", "CZK", 1.598M),
            ("USD", "CZK", 21.676M)
        });
    }
}