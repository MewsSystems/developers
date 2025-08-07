using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Mews.ExchangeRate.Provider.CNB.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Mews.ExchangeRate.Provider.CNB.UnitTests;
public class CnbExchangeRateProviderTests
{
    private Fixture _fixture;

    public CnbExchangeRateProviderTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    // TODO
    // test constructor

    [Fact]
    public async Task CheckHealthAsync_ReturnsHealthy_WhenServiceResolvesHealthCheckUrl()
    {
        //would be worth refactoring the arrange section when developing more tests
        var settings = _fixture.Build<CnbSettings>()
            .With(x => x.HealthcheckEndpoint, "http://www.contoso.com")
            .Create();
        var options = _fixture.Freeze<IOptions<CnbSettings>>();
        options.Value
            .Returns(settings);

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(settings.HealthcheckEndpoint)
            .Respond("text/javascript", "OK");

        var httpClientFactoryMock = _fixture.Freeze<IHttpClientFactory>();
        httpClientFactoryMock.CreateClient(settings.HealthcheckEndpoint)
            .Returns(mockHttp.ToHttpClient());

        var sut = _fixture.Create<CnbExchangeRateProvider>();

        var result = await sut.CheckHealthAsync(_fixture.Create<HealthCheckContext>());
        result.Status
            .Should()
            .Be(HealthStatus.Healthy);
    }

    // TODO 
    // public async Task CheckHealthAsync_ReturnsUnHealthy_WhenServiceDoesNotResolveHealthCheckUrl()
    // public async Task CheckHealthAsync_ReturnsUnHealthy_WhenServiceDoesNotReply200OK()
    // public async Task CheckHealthAsync_ReturnsUnHealthy_WhenExceptionIsThrown()
    // public async Task GetAllExchangeRatesAsync_ThrowsExchangeRateException_WhenAnyExceptionIsThrown()
}
