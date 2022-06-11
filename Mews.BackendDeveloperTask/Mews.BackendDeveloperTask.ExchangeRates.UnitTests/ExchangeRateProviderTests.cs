using System;
using NUnit.Framework;

namespace Mews.BackendDeveloperTask.ExchangeRates.UnitTests;

public class ExchangeRateProviderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ReturnsExchangeRatesForSpecifiedCurrenciesAsDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source...
        throw new NotImplementedException();
    }

    [Test]
    public void ReturnsEmptyWhenAllCurrenciesNotDefinedBySource()
    {
        // ... But only those defined by the source
        throw new NotImplementedException();
    }

    [Test]
    public void ReturnsExchangeRatesForDefinedSpecifiedCurrenciesWhenSomeNotDefinedBySource()
    {
        // Should return exchange rates among the specified currencies that are defined by the source. But only those defined by the source
        throw new NotImplementedException();
    }

    [Test]
    public void ReturnsEmptyWhenCurrencyIsTargetInSourceCurrenciesButNotItselfASource()
    {
        // Do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK" do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD".
        throw new NotImplementedException();
    }
}