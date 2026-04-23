using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Steps.Dependencies;

[Binding]
[Scope(Tag = "Cache")]
public class CacheSteps(FeatureContext featureContext)
{
    private const string _allCnbExRatesKey = "AllCnbExRates";

    private readonly IDistributedCache _cache = featureContext.Get<IServiceProvider>().GetRequiredService<IDistributedCache>();

    [Then(@"all Czech National Bank exchange rates are stored in cache")]
    public async Task ThenAllCzechNationalBankExchangeRatesAreStoredInCache()
    {
        var exchangeRatesAsString = (await _cache.GetStringAsync(_allCnbExRatesKey))!;
        var exchangeRates = JsonConvert.DeserializeObject<IEnumerable<CzechNationalBankExchangeRate>>(exchangeRatesAsString);
        exchangeRates.Should().NotBeEmpty();
    }

    [Then(@"no Czech National Bank exchange rates are added to the cache")]
    public async Task ThenNoCzechNationalBankExchangeRatesAreAddedToTheCache()
    {
        string? exchangeRatesAsString = await _cache.GetStringAsync(_allCnbExRatesKey);
        exchangeRatesAsString.Should().BeNull();
    }
}
