using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Hooks;

[Binding]
public static class BeforeFeatureSteps
{
    [BeforeFeature]
    public static void BeforeFeature(FeatureContext featureContext)
    {
        featureContext.Set(TestHost.ServiceProvider);
    }
}
