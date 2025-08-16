using ExchangeRateUpdater.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.ApiTests.Setup;

public class ExchangeRateUpdaterApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;
    public MockHttpMessageHandler ExchangeRateServiceMockHttpHandler = new();

    public async Task InitializeAsync()
    {
        await Task.Run(() => HttpClient = CreateClient());

    }

    Task IAsyncLifetime.DisposeAsync() => DisposeAsync().AsTask();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddHttpClient<ExternalExchangeRateProviderHttpClient>()
                .ConfigurePrimaryHttpMessageHandler(() => ExchangeRateServiceMockHttpHandler);
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
        client.DefaultRequestHeaders.Add("X-Api-Key", "UserSecretKey!");
    }
}
