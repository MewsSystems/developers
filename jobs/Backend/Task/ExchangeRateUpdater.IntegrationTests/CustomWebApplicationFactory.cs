using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace ExchangeRateUpdater.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IExchangeRateApiClient> ApiClient;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IExchangeRateApiClient>();

            ApiClient = new Mock<IExchangeRateApiClient>();
            ApiClient.Setup(x => x.GetAllExchangeRatesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CnbExchangeRate>()
                {
                    new("EUR", 20, 1),
                    new("USD", 30, 10)
                });
            
            services.AddSingleton(ApiClient.Object);
        });
    }
}