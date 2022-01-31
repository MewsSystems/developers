using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

// ReSharper disable UnusedMember.Global

namespace ExchangeRateUpdater.Commands
{
    [Command]
    public class PrintExchangeRatesCommand : ICommand
    {
        private static readonly IEnumerable<Currency> Currencies = new[]
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

        private readonly IExchangeRateProvider _exchangeRateProvider;

        public PrintExchangeRatesCommand(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }
        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                var rates = (await _exchangeRateProvider.GetExchangeRates(Currencies)).ToArray();

                await console.Output.WriteLineAsync($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    WriteExchangeRate(rate, console);
                }
            }
            catch (Exception e)
            {
                await console.Output.WriteLineAsync($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            await console.Input.ReadLineAsync();
        }


        private static void WriteExchangeRate(ExchangeRate rate, IConsole console)
        {
            console.ForegroundColor = ConsoleColor.Green;
            console.Output.Write(rate.SourceCurrency);
            console.ResetColor();
            console.Output.Write("/");
            console.ForegroundColor = ConsoleColor.DarkYellow;
            console.Output.Write(rate.TargetCurrency);
            console.ResetColor();
            console.Output.Write(" = ");
            console.ForegroundColor = ConsoleColor.Red;
            console.Output.Write(rate.Value);
            console.ResetColor();
            console.Output.Write(Environment.NewLine);
        }
    }
}
