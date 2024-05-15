using Adpater.Http.CzechNationalBank;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExchangeRateUpdater.Application
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // create hosting object and DI layer
                using IHost host = CreateHostBuilder(args).Build();

                // create a service scope
                using var scope = host.Services.CreateScope();

                var services = scope.ServiceProvider;

                var settings = services.GetRequiredService<ExangeRateUpdaterSettings>();
                var service = services.GetRequiredService<IExchangeRateService>();

                var currenciesToExchange = settings.CurrenciesToExchange.Select(x => new CurrencyDto(x)).ToList();
                var rates = await service.GetDailyExchangeRateForCurrencies(settings.ExchangeFrom, currenciesToExchange , default(CancellationToken));

                System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    System.Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            System.Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] strings)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    var configurationRoot = ctx.Configuration;
                    var settings = configurationRoot.GetSection(nameof(ExangeRateUpdaterSettings)).Get<ExangeRateUpdaterSettings>();

                    services.AddSingleton(settings);
                    services.AddSingleton<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
                    services.AddSingleton<IExchangeRateService, ExchangeRateService>();
                    services.AddSingleton<IGetDailyExchangeRateUseCase, GetDailyExchangeRateUseCase>();
                    services.AddSingleton<ApiRetryPolicy>();

                    services.AddHttpClient("CzechNationalBankApi", c =>
                    {
                        c.BaseAddress = new Uri(settings.ExchangeRateProviderSettings.BaseUrl);
                        c.Timeout = TimeSpan.FromSeconds(settings.ExchangeRateProviderSettings.Timeout);
                    })
                    .AddPolicyHandler((sp, _) => sp.GetRequiredService<ApiRetryPolicy>().GetRetryPolicy());
                })
                .UseSerilog((ctx, cfg) =>
                {
                    cfg.Enrich
                        .FromLogContext()
                        .WriteTo.Console();
                });
                
        }
    }
}