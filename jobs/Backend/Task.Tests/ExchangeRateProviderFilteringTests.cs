using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExchangeRateUpdater;
using System.Threading.Tasks;
using Xunit;

public class ExchangeRateProviderFilteringTests
{
    [Fact]
    public void GetFilteredRates_returns_rates_for_requested_source_currency()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var eur = new Currency("EUR");
        var cnbRates = new List<ExchangeRate>
        {
            new ExchangeRate(usd, czk, 25.0m),
            new ExchangeRate(eur, czk, 24.0m),
        };

        var filteredRates = InvokeGetFilteredRates(cnbRates, new List<Currency> { usd }).ToList();

        Assert.Single(filteredRates);
        Assert.Equal("USD", filteredRates[0].SourceCurrency.Code);
        Assert.Equal("CZK", filteredRates[0].TargetCurrency.Code);
    }

    [Fact]
    public void GetFilteredRates_ignores_unknown_requested_currencies()
    {
        var usd = new Currency("USD");
        var cnbRates = new List<ExchangeRate>
        {
            new ExchangeRate(usd, new Currency("CZK"), 25.0m),
        };

        var filteredRates = InvokeGetFilteredRates(cnbRates, new List<Currency> { new Currency("XYZ") });

        Assert.Empty(filteredRates);
    }

    [Fact]
    public void GetFilteredRates_does_not_create_inverse_pairs()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var cnbRates = new List<ExchangeRate>
        {
            new ExchangeRate(usd, czk, 25.0m),
        };

        var filteredRates = InvokeGetFilteredRates(cnbRates, new List<Currency> { usd }).ToList();

        Assert.Single(filteredRates);
        Assert.DoesNotContain(filteredRates, r => r.SourceCurrency.Code == "CZK" && r.TargetCurrency.Code == "USD");
    }

    private static IEnumerable<ExchangeRate> InvokeGetFilteredRates(
        IEnumerable<ExchangeRate> cnbRates,
        IEnumerable<Currency> currencies)
    {
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource());
        var method = typeof(ExchangeRateProvider).GetMethod("GetFilteredRates", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(method);

        var result = method!.Invoke(provider, new object[] { cnbRates, currencies });
        Assert.NotNull(result);
        return (IEnumerable<ExchangeRate>)result!;
    }

    private sealed class FakeExchangeRateSource : IExchangeRateSource
    {
        private readonly IReadOnlyList<ExchangeRate> _rates;

        public FakeExchangeRateSource()
            : this(Enumerable.Empty<ExchangeRate>())
        {
        }

        public FakeExchangeRateSource(IEnumerable<ExchangeRate> rates)
        {
            _rates = rates.ToList();
        }

        public Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            return Task.FromResult<IEnumerable<ExchangeRate>>(_rates);
        }
    }
}