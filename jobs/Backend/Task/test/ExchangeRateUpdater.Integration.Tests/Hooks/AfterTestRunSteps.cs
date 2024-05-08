using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Hooks;

[Binding]
public static class AfterTestRunSteps
{
    [AfterTestRun]
    public static async Task StopTestServerAsync()
    {
        await TestHost.StopTestServerAsync();
    }
}