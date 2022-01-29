using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
            
            using var host = hostBuilder.Build();
            
            // Execute service to obtain exchange rates from Czech National Bank
            ExecuteExchangeRateService(host);
        }
        
        static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            // Add Czech National Bank Api configurations
            var configuration = hostContext.Configuration;
            services.AddSingleton<ICzechNationalBankApiConfigurationProvider>(c =>
                new CzechNationalBankApiConfigurationProvider(
                    CzechNationalBankConfiguration.GetCzechNationalBankConfiguration(configuration)));
            
            // Add DI scopes
            services.AddSingleton<ICzechNationalBankApiClient, CzechNationalBankApiClient>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddSingleton<IExchangeRateService, ExchangeRateService>();
        }

        static void ExecuteExchangeRateService(IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();
            exchangeRateService.Execute();
        }
    }
}
