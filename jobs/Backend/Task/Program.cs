using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Domain.Validators;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                }) .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;
                    services.AddOptions<ExchangeRateProviderOptions>()
                        .Bind(configuration.GetSection("ExchangeRateProvider"))
                        .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl), "BaseUrl must be provided in ExchangeRateProvider configuration.")
                        .Validate(options => options.BaseUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase),
                            "BaseUrl must start with 'http'.")
                        .ValidateOnStart();
                    
                    services.AddOptions<CurrencyOptions>()
                        .Bind(configuration.GetSection("CurrencyOptions"))
                        .Validate(options => options.Currencies != null && options.Currencies.Length > 0, "Currencies list cannot be empty")
                        .ValidateOnStart();
                    
                    services.AddHttpClient();
                    
                    services.AddSingleton<IExchangeRateValidator, ExchangeRateValidator>();
                    services.AddSingleton<IExchangeRateMapper, ExchangeRateMapper>();
                    
                    services.AddTransient<IExchangeRateProvider>(sp =>
                    {
                        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                        var httpClient = httpClientFactory.CreateClient();
                        var options = sp.GetRequiredService<IOptions<ExchangeRateProviderOptions>>();
                        var validator = sp.GetRequiredService<IExchangeRateValidator>();
                        var mapper = sp.GetRequiredService<IExchangeRateMapper>();
                        return new CzechNationalBankExchangeRateProvider(httpClient, validator, mapper, options);
                    });
                    
                    services.AddTransient<IExchangeRateUpdaterService, ExchangeRateUpdaterService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
            
            var updaterService = host.Services.GetRequiredService<IExchangeRateUpdaterService>();
            await updaterService.RunAsync();
        }
    }
}
