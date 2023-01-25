using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        // Keeping this simple. Let's suppose that we need to get always the rates for these currencies.
        // If not, we could pass them as parameters, load them from a file... whatever. The important thing here is the provider.
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

        private static IConfiguration _config;
        public static async Task Main(string[] args)
        {
            try
            {
                // Config file to get the parameters
                var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _config = builder.Build();

                List<ExchangeRate> rates = await GetRates();

                // Result
                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        private static async Task<List<ExchangeRate>> GetRates()
        {
            // I decided to use an Azure Function. It could be a regular web API... but since we talked about serverless,
            // service bus, logic apps and azure services, makes sense to do it in this way.
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("TargetCurrency", "CZK");
            client.BaseAddress = new Uri(_config["ExchangeRateBaseAddress"]);

            string request = JsonConvert.SerializeObject(currencies);
            var apiResponse = await client.PostAsync(_config["ExchangeRateApi"], new StringContent(request));

            // Reading the content from the API
            var content = await apiResponse.Content.ReadAsStringAsync();
            var rates = JsonConvert.DeserializeObject<List<ExchangeRate>>(content);
            return rates;
        }
    }
}
