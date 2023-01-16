using ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExchangeRateUpdater.WebApi.Tests;

public class IntegrationTest 
{
    internal readonly HttpClient? TestClient;

    public IntegrationTest()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>().
            WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(IExchangeRateParser));
                    services.Add(new ServiceDescriptor(typeof(IExchangeRateParser), typeof(ExchangeRateParser), ServiceLifetime.Scoped));
                });
            });
        TestClient = webApplicationFactory.CreateClient();
    }

    
}