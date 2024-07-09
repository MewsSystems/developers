using ExchangeRateUpdater.Integration.Tests.Contexts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Hooks;

[Binding]
public static class BeforeScenarioSteps
{
    private const string _allCnbExRatesKey = "AllCnbExRates";

    [BeforeScenario]
    public static void BeforeScenario(MockFeatureContext mockFeatureContext, FeatureContext featureContext)
    {
        mockFeatureContext.ResetMocks();
        ClearCache(featureContext);
    }

    private static void ClearCache(FeatureContext featureContext)
    {
        var cache = featureContext.Get<IServiceProvider>().GetRequiredService<IDistributedCache>();
        cache.Remove(_allCnbExRatesKey);
    }
}
