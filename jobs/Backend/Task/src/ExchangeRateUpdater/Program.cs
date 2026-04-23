using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions<CNBConfigurationOptions>()
                        .Bind(context.Configuration.GetSection(CNBConfigurationOptions.SectionName))
                        .ValidateOnStart();

                    services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                    services.AddTransient<IExchangeRateRunner, ExchangeRateRunner>();

                    services.AddHttpClient();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();

            var service = host.Services.GetRequiredService<IExchangeRateRunner>();
            service.Run();

            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
