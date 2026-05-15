using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater;
using System.Threading.Tasks;
using Xunit;

public class ExchangeRateProviderTests
{
    [Fact]
    public void GetExchangeRates_WhenSourceReturnsNoRates_ReturnsEmpty()
    {
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource());
        var rates = provider.GetExchangeRates(new List<Currency> { new Currency("USD") });

        Assert.Empty(rates);
    }

    [Fact]
    public void GetExchangeRates_WhenOneExistingCurrencyRequested_ReturnsThatRate()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var sourceRates = new[] { new ExchangeRate(usd, czk, 25m) };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { usd }).ToList();

        Assert.Single(rates);
        Assert.Same(usd, rates[0].SourceCurrency);
        Assert.Same(czk, rates[0].TargetCurrency);
        Assert.Equal(25m, rates[0].Value);
    }

    [Fact]
    public void GetExchangeRates_WhenRequestedCurrencyHasSameCodeAsSourceCurrency_ReturnsThatRate()
    {
        var sourceRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25m),
        };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { new Currency("USD") }).ToList();

        Assert.Single(rates);
        Assert.Equal("USD", rates[0].SourceCurrency.Code);
        Assert.Equal("CZK", rates[0].TargetCurrency.Code);
        Assert.Equal(25m, rates[0].Value);
    }

    [Fact]
    public void GetExchangeRates_WhenNoRateMatchesRequest_ReturnsEmpty()
    {
        var gbp = new Currency("GBP");
        var jpy = new Currency("JPY");
        var sourceRates = new[] { new ExchangeRate(gbp, jpy, 150m) };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { new Currency("USD") });

        Assert.Empty(rates);
    }

    [Fact]
    public void GetExchangeRates_WhenOnlyUnknownRequestedCurrencies_ReturnsEmpty()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var sourceRates = new[] { new ExchangeRate(usd, czk, 25m) };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { new Currency("XYZ") });

        Assert.Empty(rates);
    }

    [Fact]
    public void GetExchangeRates_WhenMultipleRequested_IncludesOnlyRatesWhoseSourceCurrencyWasRequested()
    {
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var czk = new Currency("CZK");
        var sourceRates = new[]
        {
            new ExchangeRate(usd, czk, 25m),
            new ExchangeRate(eur, czk, 24m),
        };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { usd }).ToList();

        Assert.Single(rates);
        Assert.Same(usd, rates[0].SourceCurrency);
    }

    [Fact]
    public void GetExchangeRates_WhenMultipleSourceCurrenciesRequested_ReturnsRatesForThoseSourcesOnly()
    {
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var jpy = new Currency("JPY");
        var czk = new Currency("CZK");

        var sourceRates = new[]
        {
            new ExchangeRate(usd, czk, 25m),
            new ExchangeRate(eur, czk, 24m),
            new ExchangeRate(jpy, czk, 0.15m),
        };

        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new[] { usd, eur }).ToList();

        Assert.Equal(2, rates.Count);
        Assert.Contains(rates, r => r.SourceCurrency == usd && r.TargetCurrency == czk);
        Assert.Contains(rates, r => r.SourceCurrency == eur && r.TargetCurrency == czk);
        Assert.DoesNotContain(rates, r => r.SourceCurrency == jpy && r.TargetCurrency == czk);
    }

    [Fact]
    public void GetExchangeRates_WhenMixOfKnownAndUnknownRequested_ReturnsRatesForKnownOnly()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var sourceRates = new[] { new ExchangeRate(usd, czk, 25m) };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { usd, new Currency("XYZ") }).ToList();

        Assert.Single(rates);
        Assert.Same(usd, rates[0].SourceCurrency);
    }

    [Fact]
    public void GetExchangeRates_WhenSourcePublishesSingleDirection_DoesNotSynthesizeInversePair()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var sourceRates = new[] { new ExchangeRate(usd, czk, 25m) };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency> { usd }).ToList();

        Assert.Single(rates);
        Assert.Contains(rates, r => r.SourceCurrency == usd && r.TargetCurrency == czk);
        Assert.DoesNotContain(rates, r => r.SourceCurrency == czk && r.TargetCurrency == usd);
    }

    [Fact]
    public void GetExchangeRates_WhenRequestIsEmpty_ReturnsEmptyEvenIfSourceHasRates()
    {
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        var sourceRates = new[] { new ExchangeRate(usd, czk, 25m) };
        var provider = new ExchangeRateProvider(new FakeExchangeRateSource(sourceRates));

        var rates = provider.GetExchangeRates(new List<Currency>());

        Assert.Empty(rates);
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
