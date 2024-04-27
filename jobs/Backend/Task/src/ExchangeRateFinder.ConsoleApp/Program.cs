using ExchangeRateFinder.ConsoleApp.ApiClients;
using ExchangeRateFinder.ConsoleApp.Requests.Models;
using ExchangeRateFinder.ConsoleApp.Responses.Models;
using ExchangeRateFinder.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> Currencies = new[]
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

        private static string SourceCurrencyCode = "CZK";

        static async Task Main(string[] args)
        {

            // Sleep for 5 seconds to ensure the API is running
            WaitForApiInitializationAsync();

            // Load configuration from appsettings.json
            var configuration = LoadConfiguration();

            var apiUrl = ConstructApiUrl(configuration.GetSection("ExchangeRateFinderAPI")["Url"], SourceCurrencyCode, Currencies);

            HttpClientService apiClientService = new HttpClientService();
            try
            {
                var response = await apiClientService.GetCalculatedExchangeRatesAsync(apiUrl);
                var exchangeRates = JsonConvert.DeserializeObject<List<CalculatedExchangeRate>>(response);

                foreach (var exchangeRate in exchangeRates)
                {
                    Console.WriteLine(exchangeRate.ToString());
                }

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static IConfigurationRoot LoadConfiguration()
        {
            return new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .Build();
        }

        private static void WaitForApiInitializationAsync()
        {
            Thread.Sleep(5000);
        }

        private static string ConstructApiUrl(string apiUrl, string sourceCurrencyCode, IEnumerable<Currency> currencies)
        {
            var currencyCodes = string.Join(",", currencies.Select(c => c.Code));
            return $"{apiUrl}/exchange-rates?sourceCurrencyCode={sourceCurrencyCode}&targetCurrencyCodes={currencyCodes}";
        }
    }
}