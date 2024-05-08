using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace ExchangeRateUpdater.Integration.Tests;

public static class TestHost
{
    private const string _appSettingFileName = "appsettings.IntegrationTest.json";

    private static readonly IHost _testServer = GetWebHostBuilder();

    public static IServiceProvider ServiceProvider => _testServer.Services;

    public static async Task StopTestServerAsync()
    {
        var stopTask = _testServer.StopAsync();
        if (stopTask is not null)
            await stopTask;

        _testServer.Dispose();
    }

    public static HttpClient Client => _testServer.GetTestClient();

    private static IHost GetWebHostBuilder()
    {
        return new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        config.Sources.Clear();
                        config.AddJsonFile(_appSettingFileName, optional: false);
                    })
                    .UseEnvironment("Development")
                    .UseStartup<Startup>()
                    .ConfigureTestServices(services =>
                    {
                        services
                            .RemoveHostedServices()
                            .ConfigureClientsMock()
                            .ConfigureFeatureFlagsMock();
                    });
            }) 
            .Start();
    }
}
