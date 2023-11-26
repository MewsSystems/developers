using Adapter.InMemory.Repositories;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace ExchangeRateUpdaterApi.Tests.Unit.Controllers;

public class TestBase : IDisposable
{
    private IHost _host;
    protected HttpClient HttpClient { get; set; }
    protected ExchangeRatesRepositoryInMemory ExchangeRatesRepositoryInMemory { get; private set; }
    
    [SetUp]
    public async Task StartHost()
    {
        var testApplicationHostBuilder = new TestApplicationHostBuilder(null, "TestApplication");

        _host = testApplicationHostBuilder.BuildHost(c => c.UseTestServer());

        ExchangeRatesRepositoryInMemory = new ExchangeRatesRepositoryInMemory();

        testApplicationHostBuilder.RegisterDependencies(ExchangeRatesRepositoryInMemory);

        await _host.StartAsync();
        HttpClient = _host.GetTestClient();
    }
    
    public void Dispose()
    {
        HttpClient.Dispose();

        _host.StopAsync();
        _host.WaitForShutdownAsync();
        _host.Dispose();
    }

    [TearDown]
    public Task StopHost()
    {
        this.Dispose();
        return Task.CompletedTask;
    }
}