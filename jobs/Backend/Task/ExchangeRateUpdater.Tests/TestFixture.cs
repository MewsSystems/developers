using ExchangeRateUpdater.Domain.ApiClients.Interfaces;
using ExchangeRateUpdater.Domain.Services.Implementations;
using ExchangeRateUpdater.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Tests;

public class TestFixture
{
    public IExchangeRateProvider ExchangeRateProvider { get; }
    public IServiceProvider Services { get; }
    public IExchangeRateApiClient ApiClientMock { get; }
    public IExchangeRateParser ParserMock { get; }

    public TestFixture()
    {
        var services = new ServiceCollection();

        services.AddDistributedMemoryCache();

        ApiClientMock = Substitute.For<IExchangeRateApiClient>();
        ParserMock = Substitute.For<IExchangeRateParser>();

        services.AddSingleton(ApiClientMock);
        services.AddSingleton(ParserMock);
        services.AddSingleton(Substitute.For<ILogger<ExchangeRateUpdaterProvider>>());
        services.AddSingleton(Substitute.For<ILogger<XmlExchangeRatesParser>>());

        services.AddSingleton<IExchangeRateProvider, ExchangeRateUpdaterProvider>();

        Services = services.BuildServiceProvider();
        ExchangeRateProvider = Services.GetRequiredService<IExchangeRateProvider>();
    }
}