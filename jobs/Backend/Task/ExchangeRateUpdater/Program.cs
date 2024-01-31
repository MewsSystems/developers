using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Services;
using Mews.Integrations.Cnb.Setup;
using Mews.Shared.Setup;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                (context, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices(
                (context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .Build();

            host.Run();
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSystemUtcClock();
            services.AddCnbIntegration(configuration.GetSection("Cnb"));
            services.AddHostedService<ExchangeRateUpdaterJob>();
        }
    }
}
