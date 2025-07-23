using ExchangeRateUpdater.Services;
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
                .ConfigureServices((context, services) =>
                {
                    // Register application services
                    services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                    services.AddTransient<IExchangeRateRunner, ExchangeRateRunner>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();

            // Resolve and run the service
            var service = host.Services.GetRequiredService<IExchangeRateRunner>();
            service.Run();

            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
