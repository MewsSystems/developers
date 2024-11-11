using ExchangeRate.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace ExchangeRate.IntegrationTests;

public class ApiTestBase : IDisposable
{
    protected readonly HttpClient Client;

    protected ApiTestBase()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.ConfigureServices();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.ConfigureApplication();
        app.Start();

        Client = app.GetTestClient();
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}