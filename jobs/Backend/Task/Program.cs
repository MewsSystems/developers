using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient;
using ExchangeRateUpdater.HostedServices;
using ExchangeRateUpdater.Providers.ExchangeRateProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build();

            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }
        
        static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            // Add Czech National Bank Api configurations
            var configuration = hostContext.Configuration;
            services.AddSingleton<ICzechNationalBankApiConfigurationProvider>(c =>
                new CzechNationalBankApiConfigurationProvider(
                    GetCzechNationalBankConfiguration(configuration)));
            
            // Add DI scopes
            services.AddSingleton<ICzechNationalBankApiClient, CzechNationalBankApiClient>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            
            // Add hosted services
            services.AddHostedService<ExchangeRateHostedService>();
        }

        static CzechNationalBankConfiguration GetCzechNationalBankConfiguration(IConfiguration configuration)
        {
            var czechNationalBankConfigurationName = nameof(CzechNationalBankConfiguration);
            var czechNationalBankConfiguration = new CzechNationalBankConfiguration
            {
                HttpProtocol = configuration.GetSection($"{czechNationalBankConfigurationName}:{nameof(CzechNationalBankConfiguration.HttpProtocol)}").Value,
                DomainUrl = configuration.GetSection($"{czechNationalBankConfigurationName}:{nameof(CzechNationalBankConfiguration.DomainUrl)}").Value,
                Endpoints = new CzechNationalBankApiClientEndpoints
                {
                    ExchangeRatePath = configuration.GetSection($"{czechNationalBankConfigurationName}:{nameof(CzechNationalBankConfiguration.Endpoints)}:{nameof(CzechNationalBankApiClientEndpoints.ExchangeRatePath)}").Value
                }
            };

            return czechNationalBankConfiguration;
        }
    }
}
