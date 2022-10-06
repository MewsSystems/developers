using Autofac;
using Autofac.Extensions.DependencyInjection;
using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Mews.CurrencyExchange.Console
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = SetupConfiguration();
            await Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddLogging();
                    services.AddHostedService<CurrencyExchangeApp>();
                })
                .ConfigureContainer<ContainerBuilder>(container => SetupDependencyConfiguration(container, configuration))
                .RunConsoleAsync();
        }

        private static void SetupDependencyConfiguration(ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            containerBuilder.RegisterModule(new CzechNationalBankProviderModule(configuration));
        }

        private static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
