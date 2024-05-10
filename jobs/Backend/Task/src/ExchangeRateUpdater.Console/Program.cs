using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Console;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.CzechNationalBank;
using Serilog;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

                var builder = Host.CreateApplicationBuilder();
                
                builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: false);

                var settings = new Settings(builder.Configuration);

                builder.Logging.AddConsole();
                builder.Services.AddHostedService<Worker>();
                builder.Services.AddSingleton(settings.AppConfiguration);
                builder.Services.ConfigureApplication();
                builder.Services.ConfigureCzechNationalBank(settings.CzechNationalBankConfiguration);
                builder.Services.AddSerilog(config => config.ReadFrom.Configuration(builder.Configuration));

                builder.Build().Run();
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Application failed to start: '{e.Message}'.");
            }
        }
    }
}
