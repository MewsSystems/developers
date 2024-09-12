using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCases;
using ExchangeRateUpdater.Host.WebApi.Configuration;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.CacheTests;
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
        private ReferenceTime ReferenceTime { get; }
        

        public TestApplicationHostBuilder(ExchangeRateProviderRepositoryInMemory exchangeRateProviderRepositoryInMemory, Settings settings, ILogger logger, 
            ReferenceTime referenceTime, ExchangeRateCacheRepositoryInMemory? exchangeRateCacheRepositoryInMemory = null) 
            : base(settings, logger)
        {
            ExchangeRateProviderRepositoryInMemory = exchangeRateProviderRepositoryInMemory;
            ExchangeRateCacheRepositoryInMemory = exchangeRateCacheRepositoryInMemory;
            ReferenceTime = referenceTime;
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

        protected override void RegisterDomainDependencies(IServiceCollection services)
        {
            services.AddSingleton<ExchangeUseCase>();
            services.AddSingleton<ReferenceTime>(ReferenceTime);
        }
    }
}
