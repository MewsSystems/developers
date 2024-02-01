using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class CustomWebApplicationFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var cacheMock = new Mock<ICacheService>();
            cacheMock
                .Setup(m => m.GetDailyExchangeRatesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((CnbDailyExchangeRatesResponse)null);

            builder.ConfigureServices(services =>
            {
                var cacheContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICacheService));
                services.Remove(cacheContextDescriptor);

                services.AddSingleton<ICacheService>(cacheMock.Object);
            });

            builder.UseEnvironment("Development");
        }
    }
}
