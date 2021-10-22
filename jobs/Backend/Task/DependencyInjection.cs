using Infrastructure.Client;
using Infrastructure.Entities.Json;
using Infrastructure.Entities.Xml;
using Infrastructure.Serializers;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using System.IO;

namespace ExchangeRateUpdater
{
    public static class DependencyInjection
    {
        private const string AppSettings = "appsettings";
        private const string Environment = "ASPNETCORE_ENVIRONMENT";
        public static IHost _host;

        public static void Configure()
        {
            var builder = new ConfigurationBuilder();
            BuildAppSettings(builder);

            Log.Logger = ConfigureLogger(builder);

            Log.Logger.Information("Initializing Container...");

            _host = ConfigureServices();
        }

        static void BuildAppSettings(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppSettings}.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"{AppSettings}.{System.Environment.GetEnvironmentVariable(Environment) ?? "Production"}.json",
                    optional: true)
                .AddEnvironmentVariables();
        }

        private static Logger ConfigureLogger(ConfigurationBuilder builder)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static IHost ConfigureServices()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IClient, WebClient>();
                    services.AddTransient<IExchangeRateService, ExchangeRateService>();
                    services.AddTransient<IDeserializer<ExchangeRateModel>, JsonDeserializer<ExchangeRateModel>>();
                    services.AddTransient<IDeserializer<KurzyModel>, XmlDeserializer<KurzyModel>>();
                })
                .UseSerilog()
                .Build();
        }
    }
}
