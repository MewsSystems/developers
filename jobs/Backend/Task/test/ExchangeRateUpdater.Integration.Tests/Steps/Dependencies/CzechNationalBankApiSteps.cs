using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;
using ExchangeRateUpdater.Integration.Tests.Contexts;
using ExchangeRateUpdater.Integration.Tests.TestData;
using Moq;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Steps.Dependencies;

[Binding]
[Scope(Tag = "CzechNationalBankApi")]
public class CzechNationalBankApiSteps(MockFeatureContext mockFeatureContext)
{
    private readonly Mock<ICzechNationalBankApiClient> _apiClient = mockFeatureContext.GetMock<ICzechNationalBankApiClient>();

    [Given(@"Czech National Bank Api is able to retrieve the exchange rates")]
    public void GivenCzechNationalBankApiIsAbleToRetrieveTheExchangeRates()
    {
        var exchangeRates = ExchangeRatesTestData.GetCzechNationalBankExchangeRates();
        _apiClient
            .Setup(c => c.GetExchangeRatesAsync(DateTime.UtcNow.Date.ToString("yyyy-MM-dd"), "EN"))
            .ReturnsAsync(new GetCzechNationalBankExchangeRatesResponse { Rates = exchangeRates });
    }

    [Given(@"Czech National Bank Api fails to retrieve the exchange rates")]
    public void GivenCzechNationalBankApiFailsToRetrieveTheExchangeRates()
    {
        _apiClient
            .Setup(c => c.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
    }

    [Given(@"Czech National Bank Api times out when retrieving the exchange rates")]
    public void GivenCzechNationalBankApiTimesOutWhenRetrievingTheExchangeRates()
    {
        _apiClient
            .Setup(c => c.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new TaskCanceledException());
    }

    [Then(@"a call to Czech National Bank Api to get the exchange rates is done")]
    [Then(@"only one call to Czech National Bank Api to get the exchange rates is done")]
    public void ThenACallToCzechNationalBankApiToGetTheExchangeRatesIsDone()
    {
        _apiClient.Verify(c => c.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Then(@"no call is performed to Czech National Bank Api to get the exchange rates")]
    public void ThenNoCallIsPerformedToCzechNationalBankApiToGetTheExchangeRates()
    {
        _apiClient.Verify(c => c.GetExchangeRatesAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
