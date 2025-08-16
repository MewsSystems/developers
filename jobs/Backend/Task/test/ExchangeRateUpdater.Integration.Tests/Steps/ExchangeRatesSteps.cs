using ExchangeRateUpdater.Integration.Tests.Contexts;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Steps;

[Binding]
[Scope(Tag = "ExchangeRates")]
public class ExchangeRatesSteps(ScenarioContext scenarioContext, HttpScenarioContext httpFeatureContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;
    private readonly HttpScenarioContext _httpFeatureContext = httpFeatureContext;

    [When(@"all exchange rates are requested")]
    public async Task WhenAllExchangeRatesAreRequested()
    {
        HttpResponseMessage response = await _httpFeatureContext.Client.GetAsync($"api/exchange-rates");
        _scenarioContext.Set(response);
    }
}
