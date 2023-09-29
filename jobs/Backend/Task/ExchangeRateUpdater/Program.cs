using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbRates;
using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).RunConsoleAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .UseConsoleLifetime()
        .ConfigureServices((hostContext, services) => services
            .AddExchangeRate(hostContext.Configuration.GetSection("ExchangeRate"))
            .AddHostedService<ExchangeRateService>()
            .AddSingleton(Console.Out));

    private static IServiceCollection AddExchangeRate(this IServiceCollection serviceCollection, IConfiguration  exchangeRateSection)
    {
        serviceCollection
            .Configure<ExchangeRateOptions>(exchangeRateSection)
            .AddTransient<IExchangeRateProvider, CnbExchangeRatesProvider>()
            .AddRefitClient<ICnbClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.cnb.cz"));

        return serviceCollection;
    }
}