using System;
using System.IO;
using System.Threading.Tasks;
using CliFx;
using ExchangeRateUpdater.Commands;
using ExchangeRateUpdater.Sources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;


namespace ExchangeRateUpdater
{

    public static class Program
    {
        public static IConfiguration Configuration { get; private set; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static async Task<int> Main()
        {
            Logger.Trace(">>>>> You can disable these entries in NLog.config -> minlevel=\"Info\" ");
            var services = new ServiceCollection();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            services.AddSingleton(Configuration);

            
            services.AddScoped<IExchangeRateProvider, ExchangeRateProviderCzechBank>();
            services.AddScoped<IExchangeRateParser, ExchangeRateParserCzechBank>();
            services.AddTransient<PrintExchangeRatesCommand>();

            var serviceProvider = services.BuildServiceProvider();
            Logger.Trace("Finished registering services");

            return await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(serviceProvider.GetService)
                .Build()
                .RunAsync();
        }
    }


}
