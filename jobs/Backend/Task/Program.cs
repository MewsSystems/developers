using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        public static async Task Main(string[] args)
        {
            try
            {
                //var confBuilder = new ConfigurationBuilder();

                // confBuilder.SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //    .AddEnvironmentVariables();

                var confBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

                var configuration = confBuilder.Build();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    //.WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "C:\\logs\\log-{Date}.txt"),
                    //                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}")
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                //Log.Logger = new LoggerConfiguration()
                //                .ReadFrom.Configuration(confBuilder.Build())
                //                .WriteTo.Console()
                //                .CreateLogger();

                Log.Logger.Information("starting serilog in a console app...");

                var builder = new HostBuilder()
                   .ConfigureServices((hostContext, services) =>
                   {
                       services.AddHttpClient();
                       services.AddSingleton<ILogger>(x => Log.Logger);
                       services.AddSingleton<ExchangeRateProvider>();
                       services.AddSingleton<IExchangeRateProvider>
                            (x => new ExchangeRateProviderWithCaching(x.GetRequiredService<ExchangeRateProvider>()));
                   })
                   .UseSerilog()
                   .UseConsoleLifetime();


                var host = builder.Build();
                var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
