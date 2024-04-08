using ExchangeRateProvider;
using ExchangeRateProvider.Constants;
using ExchangeRateProvider.Interfaces;
using ExchangeRateProvider.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate.App
{
    internal class Program
    {
        private static readonly CurrencyModel[] _supportedCurrencies =
        [
            new CurrencyModel(Currency.Czk),
            new CurrencyModel(Currency.Euro),
            new CurrencyModel(Currency.Dollar),
            new CurrencyModel(Currency.Jpy),
            new CurrencyModel(Currency.Kes),
            new CurrencyModel(Currency.Rub),
            new CurrencyModel(Currency.Thb),
            new CurrencyModel(Currency.Try),
            new CurrencyModel(Currency.Xyz)
        ];

        private static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddExchangeRateProvider();
            using var host = builder.Build();
            var cnbExchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();

            try
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var exchangeRates = await cnbExchangeRateProvider.GetExchangeRatesAsync(_supportedCurrencies, cancellationTokenSource.Token);

                LogConsoleMessage($"Successfully retrieved {exchangeRates.Length} exchange rates", ConsoleColor.Green);

                foreach (var exchangeRate in exchangeRates)
                {
                    LogConsoleMessage(exchangeRate.ToString(), ConsoleColor.White);
                }
            }
            catch (Exception ex) 
            {
                LogConsoleMessage($"There was an error processing exchange rates: {ex.Message}", ConsoleColor.Red);
            }

            await host.RunAsync();
        }

        private static void LogConsoleMessage(string message, ConsoleColor foregroundColor) 
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}