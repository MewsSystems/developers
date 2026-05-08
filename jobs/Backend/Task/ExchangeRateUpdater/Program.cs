using ExchangeRateUpdater.Application.Menu.ExchangeRateUpdater.Application.Menu;
using ExchangeRateUpdater.Application.Providers;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Infrastructure.Options;
using ExchangeRateUpdater.Infrastructure.Policies;
using ExchangeRateUpdater.Infrastructure.Sources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<CnbApiOptions>(context.Configuration.GetSection("CnbApi"));

                services.AddHttpClient<CnbApiExchangeRateSource>((sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<CnbApiOptions>>().Value;
                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(10);
                })
                .AddPolicyHandler(CnbHttpPolicies.RetryPolicy);

                services.AddScoped<IExchangeRateSource>(sp => sp.GetRequiredService<CnbApiExchangeRateSource>());
                services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddScoped<MenuHandler>();
            })
            .Build();

            using var scope = host.Services.CreateScope();
            var menu = scope.ServiceProvider.GetRequiredService<MenuHandler>();

            await menu.RunAsync();

            await host.StopAsync();
        }
    }
}