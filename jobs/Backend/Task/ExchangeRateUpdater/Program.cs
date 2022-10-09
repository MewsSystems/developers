using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace ExchangeRateUpdater;

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
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        await using var provider = GetServiceCollection(configuration).BuildServiceProvider();

        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(Program));

        try
        {
            var ratesProvider = provider.GetRequiredService<IExchangeRateProvider>();
            var rates = await ratesProvider.GetExchangeRates(currencies);

            logger.LogInformation("Successfully retrieved {NumberOfExchangeRates} exchange rates", rates.Count);
            foreach (var rate in rates)
            {
                logger.LogInformation("{SourceCurrency}/{TargetCurrency}={Value}", rate.SourceCurrency, rate.TargetCurrency, rate.Value);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not retrieve exchange rates");
        }

        Console.ReadLine();
    }

    public static IServiceCollection GetServiceCollection(IConfiguration configuration)
    {
        var services = new ServiceCollection()
            .AddLogging(builder => builder.AddSimpleConsole())
            .Configure<CzechNationalBankExchangeRateProviderOptions>(configuration.GetSection(nameof(CzechNationalBankExchangeRateProviderOptions)));

        services
            .AddHttpClient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>(HttpClientsNames.CzechNationalBank, (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<CzechNationalBankExchangeRateProviderOptions>>().Value;

                client.BaseAddress = options.BaseAddress;
            })
            .AddTransientHttpErrorPolicy(builder =>
            {
                return builder.WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryNumber => TimeSpan.FromMilliseconds(retryNumber * 100));
            });

        return services;
    }
}