using ExchangeRateFinder.ConsoleApp.ApiClients;
using ExchangeRateFinder.ConsoleApp.Requests.Models;
using ExchangeRateFinder.ConsoleApp.Responses.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateFinder.ConsoleApp
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

            // Wait for 5 seconds to ensure the API is running
            WaitForApiInitializationAsync();

            // Load configuration from appsettings.json
            var configuration = LoadConfiguration();

            // Creating http client service to make calls to the API  
            var httpClientFactory = CreateHttpClientFactory();
            var httpClientService = new HttpClientService(httpClientFactory);

            try
            {
                var endpoint = ConstructEndpoint(configuration.GetSection("ExchangeRateFinderAPI")["Url"], SourceCurrencyCode, Currencies);
                var response = await httpClientService.GetDataAsync(endpoint);

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

        private static IHttpClientFactory CreateHttpClientFactory()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            return httpClientFactory;
        }

        private static void WaitForApiInitializationAsync()
        {
            Thread.Sleep(5000);
        }

        private static string ConstructEndpoint(string apiUrl, string sourceCurrencyCode, IEnumerable<Currency> currencies)
        {
            var currencyCodes = string.Join(",", currencies.Select(c => c.Code));
            return $"{apiUrl}/exchange-rates?sourceCurrencyCode={sourceCurrencyCode}&targetCurrencyCodes={currencyCodes}";
        }
    }
}