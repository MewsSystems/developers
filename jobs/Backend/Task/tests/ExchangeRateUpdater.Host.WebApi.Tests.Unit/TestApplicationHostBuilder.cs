using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Host.WebApi.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit
{
    internal class TestApplicationHostBuilder : ApplicationHostBuilder
    {
        protected ExchangeRateProviderRepositoryInMemory ExchangeRateProviderRepositoryInMemory { get; }

        public TestApplicationHostBuilder(ExchangeRateProviderRepositoryInMemory exchangeRateProviderRepositoryInMemory) : base(new Settings(), new LoggerConfiguration().WriteTo.InMemory().CreateLogger())
        {
            ExchangeRateProviderRepositoryInMemory = exchangeRateProviderRepositoryInMemory;
        }

        

        protected override void RegisterAdapters(IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateProviderRepository>(ExchangeRateProviderRepositoryInMemory);
        }

        protected override void ConfigureServices(IWebHostBuilder webBuilder)
        {
            base.ConfigureServices(webBuilder);
            webBuilder.UseTestServer();
        }
    }
}
