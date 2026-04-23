using ExchangeRateUpdater.Integration.Tests.Contexts;
using Microsoft.FeatureManagement;
using Moq;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Steps.Common;

[Binding]
[Scope(Tag = "FeatureFlags")]
internal class FeatureFlagsSteps(MockFeatureContext mockFeatureContext)
{
    private const string IsExchangeRatesUpdaterHostedServiceEnabled = "IsExchangeRatesUpdaterHostedServiceEnabled";

    private readonly Mock<IFeatureManager> _featureManager = mockFeatureContext.GetMock<IFeatureManager>();

    [Given(@"CnbExchangeRatesUpdater is disabled")]
    public void GivenCnbExchangeRatesUpdaterIsDisabled()
    {
        SetupFeatureFlag(IsExchangeRatesUpdaterHostedServiceEnabled, false);
    }

    [Given(@"CnbExchangeRatesUpdater is enabled")]
    public void GivenCnbExchangeRatesUpdaterIsEnabled()
    {
        SetupFeatureFlag(IsExchangeRatesUpdaterHostedServiceEnabled, true);
    }

    private void SetupFeatureFlag(string featureName, bool isActive)
    {
        _featureManager.Setup(m => m.IsEnabledAsync(featureName)).ReturnsAsync(isActive);
    }

}
