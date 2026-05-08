using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.CNB;
using ExchangeRateUpdater.Services.Backup;
using ExchangeRateUpdater.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace ExchangeRateUpdater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console())
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory)
                          .AddJsonFile("appsettings.json", optional: false)
                          .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                          .AddEnvironmentVariables()
                          .AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Bind options
                    services.Configure<ExchangeRateOptions>(
                        hostContext.Configuration.GetSection("ExchangeRates"));
                    
                    services.Configure<CurrencyOptions>(
                        hostContext.Configuration.GetSection("Currency"));

                    var exchangeRateOptions = hostContext.Configuration
                        .GetSection("ExchangeRates")
                        .Get<ExchangeRateOptions>() ?? 
                        throw new InvalidOperationException("ExchangeRates configuration section is missing");

                    // Registrar el servicio de códigos ISO
                    services.AddSingleton<ICurrencyIsoService, CurrencyIsoService>();

                    // Determinar qué proveedor de datos usar basado en la configuración
                    var useBackupProvider = hostContext.Configuration.GetValue<bool>("UseBackupProvider");

                    if (useBackupProvider)
                    {
                        // Registrar el proveedor de backup
                        services.AddScoped<IExchangeRateDataProvider, BackupFileDataProvider>();
                    }
                    else
                    {
                        // Registrar el proveedor CNB con su configuración HTTP
                        services.AddHttpClient<IExchangeRateDataProvider, CNBHttpDataProvider>((serviceProvider, client) =>
                        {
                            client.BaseAddress = new Uri(exchangeRateOptions.BaseUrl);
                            client.Timeout = TimeSpan.FromSeconds(exchangeRateOptions.HttpClient.TimeoutSeconds);
                        })
                        .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
                            .WaitAndRetryAsync(
                                exchangeRateOptions.HttpClient.RetryCount,
                                retryAttempt => TimeSpan.FromSeconds(
                                    exchangeRateOptions.HttpClient.RetryWaitSeconds * retryAttempt)
                            ));
                    }

                    // Registrar servicios comunes
                    services.AddScoped<IExchangeRateParser, CNBRateParser>();
                    services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
                    services.AddHostedService<ExchangeRateService>();
                });
    }
}
