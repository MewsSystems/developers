using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Mews.ExchangeRate.Provider.Bootstrapper.Bootstrapper;
using Mews.ExchangeRate.Http.Cnb;
using Mews.ExchangeRate.Provider.Services.Abstractions;
using Mews.ExchangeRate.Domain.Models;

namespace Mews.ExchangeRate.Provider.Host;

internal static class Program
{
    private static readonly IEnumerable<Currency> currencies = new[]
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

    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(args);

        builder.Services.Configure<ExchangeRateServiceClientOptions>(builder.Configuration.GetSection(ExchangeRateServiceClientOptions.ConfigurationSectionName));
        builder.Services.AddCommonServices();

        using IHost host = builder.Build();

        await ExecuteServiceLifetime(host.Services);

        await host.RunAsync();
    }

    private static async Task  ExecuteServiceLifetime(IServiceProvider services)
    {
        try
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider.GetRequiredService<IExchangeRateService>();
            var rates = await provider.GetExchangeRatesAsync(currencies);


            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
        }

        Console.ReadLine();
    }
}
