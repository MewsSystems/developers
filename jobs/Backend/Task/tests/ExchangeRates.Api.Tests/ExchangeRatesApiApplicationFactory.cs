using ExchangeRates.Api.Tests.Extensions.ServiceCollection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRates.Api.Tests;

public class ExchangeRatesApiApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private const string _appSettingFileName = "appsettings.Test.json";

    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = new HostBuilder()
                            .ConfigureWebHost(webHostBuilder =>
                            {
                                webHostBuilder
                                    .UseTestServer()
                                    .UseEnvironment(Environments.Development)
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .ConfigureAppConfiguration((_, config) =>
                                    {
                                        config.Sources.Clear();
                                        config.SetBasePath(Directory.GetCurrentDirectory());
                                        config.AddJsonFile(_appSettingFileName, optional: false);
                                    })
                                    .UseStartup<TStartup>()
                                    .ConfigureTestServices(sp =>
                                    {
                                        var config = sp.BuildServiceProvider().GetRequiredService<IConfiguration>();
                                        sp.ConfigureWireMock(config);
                                    });
                            });

        return builder;
    }
}
