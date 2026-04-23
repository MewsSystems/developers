using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        var services = new ServiceCollection();

        services.AddLogging(b =>
        {
            b.AddSimpleConsole(o =>
            {
                o.SingleLine = true;
                o.TimestampFormat = "HH:mm:ss ";
            });
            b.SetMinimumLevel(LogLevel.Information);
        });

        services.AddHttpClient<ExchangeRateProvider>(c =>
        {
            c.Timeout = TimeSpan.FromSeconds(10);
        });

        using var provider = services.BuildServiceProvider();

        var rateProvider = provider.GetRequiredService<ExchangeRateProvider>();

        var currencies = new[]
        {
            new Currency("CZK"),
            new Currency("EUR"),
            new Currency("USD"),
            new Currency("GBP"),
        };

        try
        {
            var rates = await rateProvider
                .GetCzkBasedRatesAsync(currencies, cts.Token)
                .ConfigureAwait(false);

            foreach (var r in rates.OrderBy(r => r.TargetCurrency.Code))
            {
                Console.WriteLine(r);
            }

            return 0;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled.");
            return 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 2;
        }
    }
}
