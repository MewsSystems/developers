using Mews.Backend.Task.Core;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mews.Backend.Task.Console
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var currencies = new[]
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

            var exchangeRateParser = new TextFileExchangeRateParser(configuration["ExchangeUrl"]);
            var exchangeProvider = new CzechBankRateProvider(exchangeRateParser);
            var exchangeRates = await exchangeProvider.GetExchangeRatesAsync(currencies);

            System.Console.WriteLine($"Found {exchangeRates.Count} of {currencies.Length} provided currencies");
            exchangeRates.ForEach(x => System.Console.WriteLine(x.ToString()));
            System.Console.ReadLine();
        }
    }
}
