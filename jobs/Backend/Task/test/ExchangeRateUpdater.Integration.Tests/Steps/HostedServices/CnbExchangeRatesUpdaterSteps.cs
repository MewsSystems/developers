using ExchangeRateUpdater.Api.HostedServices;
using ExchangeRateUpdater.Infrastructure.Options;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using TechTalk.SpecFlow;
using Xunit;

namespace ExchangeRateUpdater.Integration.Tests.Steps.HostedServices;

[Binding]
[Scope(Tag = "CnbExchangeRatesUpdater")]
internal class CnbExchangeRatesUpdaterSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;
    private readonly IServiceProvider _serviceProvider = featureContext.Get<IServiceProvider>();

    [Given(@"the hosted service is executed during (.*) second")]
    [Given(@"the hosted service is executed during (.*) seconds")]
    public void GivenIsExecutedDuringSecond(int seconds)
    {
        _scenarioContext.Set(new CancellationTokenSource(TimeSpan.FromSeconds(seconds)).Token);
    }

    [When(@"CnbExchangeRatesUpdater is executed")]
    public async Task WhenCnbExchangeRatesUpdaterIsExecuted()
    {
        try
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<CnbExchangeRatesUpdater>>();
            var featureManager = _serviceProvider.GetRequiredService<IFeatureManager>();
            var hostedServiceOptions = _serviceProvider.GetRequiredService<IOptions<CnbExchangeRatesUpdaterOptions>>();

            var hostedService = new CnbExchangeRatesUpdater(logger, _serviceProvider, featureManager, hostedServiceOptions);

            var cancellationToken = _scenarioContext.Get<CancellationToken>();

            await hostedService.StartAsync(cancellationToken);

            await Task.Delay(10000, cancellationToken);
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
        {
            //Skip to stop Timed HostedServices
        }
    }

    [When(@"CnbExchangeRatesUpdater is executed no exception is thrown")]
    public async Task WhenCnbExchangeRatesUpdaterIsExecutedNoExceptionIsThrown()
    {
        Exception? exception = null;
        try
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<CnbExchangeRatesUpdater>>();
            var featureManager = _serviceProvider.GetRequiredService<IFeatureManager>();
            var hostedServiceOptions = _serviceProvider.GetRequiredService<IOptions<CnbExchangeRatesUpdaterOptions>>();

            var hostedService = new CnbExchangeRatesUpdater(logger, _serviceProvider, featureManager, hostedServiceOptions);

            var cancellationToken = _scenarioContext.Get<CancellationToken>();

            exception = await Record.ExceptionAsync(() => hostedService.StartAsync(cancellationToken));

            await Task.Delay(10000, cancellationToken);
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
        {
            //Skip to stop Timed HostedServices
        }

        exception.Should().BeNull();
    }
}
