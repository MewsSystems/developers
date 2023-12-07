using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Host.WebApi.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.InMemory;

namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit
{
    internal class TestApplicationHostBuilder : ApplicationHostBuilder
    {
        private ExchangeRateProviderRepositoryInMemory ExchangeRateProviderRepositoryInMemory { get; }
        private ExchangeRateCacheRepositoryInMemory? ExchangeRateCacheRepositoryInMemory { get; }
        

        public TestApplicationHostBuilder(ExchangeRateProviderRepositoryInMemory exchangeRateProviderRepositoryInMemory, Settings settings, ILogger logger,
            ExchangeRateCacheRepositoryInMemory? exchangeRateCacheRepositoryInMemory = null) 
            : base(settings, logger)
        {
            ExchangeRateProviderRepositoryInMemory = exchangeRateProviderRepositoryInMemory;
            ExchangeRateCacheRepositoryInMemory = exchangeRateCacheRepositoryInMemory;
        }



        protected override void RegisterAdapters(IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateProviderRepository>(ExchangeRateCacheRepositoryInMemory == null 
                                                                        ? ExchangeRateProviderRepositoryInMemory 
                                                                        : ExchangeRateCacheRepositoryInMemory);   
        }

        protected override void ConfigureServices(IWebHostBuilder webBuilder)
        {
            base.ConfigureServices(webBuilder);
            webBuilder.UseTestServer();
        }
    }
}
