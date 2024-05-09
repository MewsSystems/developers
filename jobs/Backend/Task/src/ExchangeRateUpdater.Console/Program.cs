using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.CzechNationalBankApi;
using ExchangeRateUpdater.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Console;
using ExchangeRateUpdater.Core.Providers;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = Host.CreateApplicationBuilder();

                builder.Logging.AddConsole();
                builder.Services.AddHostedService<Worker>();
                builder.Services.ConfigureApplication();
                builder.Services.ConfigureApi();
                builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

                builder.Build().Run();
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Application failed to start: '{e.Message}'.");
            }
        }
    }
}
