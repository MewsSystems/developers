using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ExchangeRates.Api.Tests;

public class ExchangeRatesTestsContext : IAsyncLifetime
{
    public readonly HttpClient HttpClient;
    public readonly IServiceProvider Services;

    private readonly ExchangeRatesApiApplicationFactory<Startup> _exchangeRatesApiApplicationFactory;

    public ExchangeRatesTestsContext()
    {
        _exchangeRatesApiApplicationFactory = new ExchangeRatesApiApplicationFactory<Startup>();

        HttpClient = _exchangeRatesApiApplicationFactory.CreateClient();
        Services = _exchangeRatesApiApplicationFactory.Services;
    }

    public Task DisposeAsync()
    {
        var wireMockServer = Services.GetRequiredService<WireMockServer>();

        wireMockServer.Stop();

        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

}
