using ExchangeRateProvider.Application;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Tests.Infrastructure
{
    public class ServiceRegistrationTests
    {

        [Fact]
        public void Registers_DistributedCache_Provider_When_Redis_Enabled()
        {
            var cfg = new ConfigurationBuilder().AddInMemoryCollection([
                new KeyValuePair<string,string?>("Redis:Enabled","true"),
                new KeyValuePair<string,string?>("Redis:Configuration","localhost:6379")
            ]).Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddApplicationServices();
            services.AddInfrastructureServices(cfg);
            var sp = services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IExchangeRateProvider>();
            Assert.IsType<DistributedCachingExchangeRateProvider>(provider);
        }
    }
}


