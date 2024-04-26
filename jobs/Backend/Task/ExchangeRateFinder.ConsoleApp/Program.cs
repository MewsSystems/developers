using ExchangeRateFinder;
using ExchangeRateFinder.Application.Extensions;
using ExchangeRateFinder.Infrastructure.Extensions;
using ExchangeRateSyncService.Extensions;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
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

        private static string sourceCurrency = "CZK";

        static async Task Main(string[] args)
        {

            // We need that since we want to make sure the API is running and have ingested initially the 
            Thread.Sleep(5000);

            string currencyCodes = string.Join(",", currencies.Select(c => c.Code));
            string apiUrl = $"https://localhost:7210/exchange-rates?sourceCurrency={sourceCurrency}&targetCurrencies={currencyCodes}";
            ExchangeRateFinderApiClient apiClient = new ExchangeRateFinderApiClient();

            try
            {
                var exchangeRateResponse = await apiClient.CallApiAsync(apiUrl);
                foreach(var exchangeRate in exchangeRateResponse)
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
    }
}