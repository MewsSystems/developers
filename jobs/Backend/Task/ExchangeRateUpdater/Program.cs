using ExchangeRateUpdater.Lib.examples;
using ExchangeRateUpdater.Lib.Shared;
using ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using V1CzechNationBankExchangeRateProvider.DependencyModule;
namespace ExchangeRateUpdater
{
    /// <summary>
    /// example: ./ExchangeRateUpdater.exe USD --mode p > test.json
    /// this example will pull USD exchange rates from all registered providers and output it to ./test.json
    /// </summary>
    public static class Program
    {
        public static void Main(
            string[] args // optionally provide a list of currencies to the command line
        )
        {
            ReadArgs(args,
                out ICollection<Currency> currencies,
                out bool usePipedOutput
            );

            var serviceCollection = new ServiceCollection()
                .AddSingleton<App, App>()// the empty provider that came with the sample
                .AddTransient<IExchangeRateProvider, ExchangeRateProvider>()// the empty provider that came with the sample
                .AddCzechNationBankExchangeRateProviderModule( // our new czech nation bank provider
                    settings: ExchangeRateProviderSettings.LoadFromAppSettings(),
                    usePipedOutput
                );


            // Build the service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider
                .GetService<App>()
                .Run(
                    currencies,
                    usePipedOutput
                );

        }

        /// <summary>
        /// read cli args
        /// </summary>
        /// <param name="args"></param>
        /// <param name="currencies">
        /// list of curencies to get rates for: ABC CDE EFG USD.
        /// when no currencies are provided we use a list of default currencies
        /// </param>
        /// <param name="usePipedOutput">are we passing the output to another application?</param>
        private static void ReadArgs(
            string[] args,
            out ICollection<Currency> currencies,
            out bool usePipedOutput
            )
        {
            // queue a list of default currencies
            currencies = GetDefaultCurrencies();

            // pretend were using this with a cron job and have received a list of codes we want to update through some other scripting
            usePipedOutput = false;

            // if args are provided, clear the demo currency codes
            if (args.Length > 0)
            {
                currencies.Clear();
            }

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--mode":
                        if (i + 1 < args.Length)
                        {
                            string mode = args[i + 1];

                            usePipedOutput = new[] { "p", "pipe" }
                                                   .Any(m => m.Equals(mode, StringComparison.OrdinalIgnoreCase));

                            i++; // Skip the next argument since it's the mode value
                        }
                        else
                        {
                            Console.Error.WriteLine("Error: --mode switch requires a value.");
                            return;
                        }
                        break;
                    default:
                        currencies.Add(new Currency(args[i]));
                        break;
                }
            }
        }

        private static ICollection<Currency> GetDefaultCurrencies()
        {
            return new[]
            {
                "USD",
                "EUR",
                "CZK",
                "JPY",
                "KES",
                "RUB",
                "THB",
                "TRY",
                "XYZ"
            }
            .Select(code => new Currency(code))
            .ToList();
        }
    }
}
