using System;
using ExchangeRateUpdater.ExchangeRateApiServiceClient;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace ExchangeRateUpdater.Specs.Hooks
{
    [Binding]
    public class Hooks
    {
        private const string EnvironmentVariableName = $"{nameof(IExchangeRateApiServiceClient)}__BaseUrl";

        [BeforeScenario]
        public void BeforeEachScenario()
        {
            TestData.Reset();
            WireMock.Reset();
            Environment.SetEnvironmentVariable(EnvironmentVariableName, $"{WireMock.BaseUrl}/exchange-rate");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, null);
        }
    }
}