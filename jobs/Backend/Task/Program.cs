using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((host, services) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .CreateBootstrapLogger();
                    
                    // Console wrapper for testing
                    services.AddSingleton<IConsoleManager, ConsoleManager>();
                    
                    services
                        .AddExchangeOptions(host.Configuration)
                        .AddCnbHttpClient()
                        .AddServices();

                    services.AddHostedService<HostedProcessingService>();
                })
                .UseSerilog((context, logConfig) =>
                    logConfig.ReadFrom.Configuration(context.Configuration))
                .Build();

            await host.RunAsync();
        }
    }
}