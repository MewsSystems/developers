using ExchangeRateUpdater.Application.Providers;
using ExchangeRateUpdater.Application.Validators;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Menu
{
    namespace ExchangeRateUpdater.Application.Menu
    {
        public class MenuHandler
        {
            private readonly IExchangeRateProvider _exchangeRateProvider;
            private readonly ILogger<MenuHandler> _logger;

            public MenuHandler(IExchangeRateProvider exchangeRateProvider, ILogger<MenuHandler> logger)
            {
                _exchangeRateProvider = exchangeRateProvider;
                _logger = logger;
            }

            public async Task RunAsync()
            {
                //Needed cause main async method -> console initializes before app sometimes
                Console.WriteLine("Starting Exchange Rate Updater...");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();

                bool exit = false;

                while (!exit)
                {
                    Console.Clear();
                    Console.WriteLine(ASCIIHelper.LoadFromAssets("Assets/ascii-cnb-title.txt"));
                    Console.WriteLine("=== Exchange Rate Updater ===");
                    Console.WriteLine("[1] Get latest exchange rates");
                    Console.WriteLine("[2] Get exchange rates for a date");
                    Console.WriteLine("[3] Compare exchange rates between 2 dates");
                    Console.WriteLine("[0] Exit");
                    Console.Write("Choose an option: ");

                    string choice = Console.ReadLine();

                    bool actionExecuted = false;

                    switch (choice)
                    {
                        case "1":
                            await GetLatestExchangeRates();
                            actionExecuted = true;
                            break;

                        case "2":
                            actionExecuted = await GetDateExchangeRates();
                            break;

                        case "3":
                            actionExecuted = await GetComparedExchangeRates();
                            break;

                        case "0":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }

                    if (actionExecuted && !exit)
                    {
                        Console.WriteLine("\nPress Enter to continue...");
                        Console.ReadLine();
                    }
                }
            }

            private async Task GetLatestExchangeRates()
            {
                Console.WriteLine("Getting latest exchange rates...");

                try
                {
                    IEnumerable<Currency> currencies = SupportedCurrencies.All;
                    IEnumerable<ExchangeRate> rates = await _exchangeRateProvider.GetExchangeRates(currencies);

                    if (rates == null || rates.Count() <= 0)
                    {
                        Console.WriteLine("No exchange rates were retrieved.");
                    }
                    else
                    {
                        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                        foreach (ExchangeRate rate in rates)
                        {
                            Console.WriteLine(rate.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Could not retrieve exchange rates: {e.Message}");
                }
            }

            private async Task<bool> GetDateExchangeRates()
            {
                Console.Write("Enter date (yyyy-MM-dd) or 'exit' to go back to menu: ");
                string date = Console.ReadLine()!;

                while (!DateValidator.ValidateDate(date))
                {
                    Console.Write("Enter date (yyyy-MM-dd) or 'exit' to go back to menu: ");
                    date = Console.ReadLine()!;
                    if (date.ToLower() == "exit")
                    {
                        return false;
                    }
                }

                Console.WriteLine($"Getting exchange rates for {date}...");

                try
                {
                    IEnumerable<Currency> currencies = SupportedCurrencies.All;
                    IEnumerable<ExchangeRate> rates = await _exchangeRateProvider.GetExchangeRatesFromDay(SupportedCurrencies.All, date);

                    if (rates == null || rates.Count() <= 0)
                    {
                        Console.WriteLine($"No exchange rates were retrieved for {date}");
                    }
                    else
                    {
                        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates for {date}:");
                        foreach (ExchangeRate rate in rates)
                        {
                            Console.WriteLine(rate.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Could not retrieve exchange rates: {e.Message}");
                }
                return true;
            }

            private async Task<bool> GetComparedExchangeRates()
            {
                Console.Write("Enter first date (yyyy-MM-dd) or 'exit' to go back to menu: ");
                string firstDate = Console.ReadLine()!;

                while (!DateValidator.ValidateDate(firstDate))
                {
                    Console.Write("Enter first date (yyyy-MM-dd) or 'exit' to go back to menu: ");
                    firstDate = Console.ReadLine()!;
                    if (firstDate.ToLower() == "exit")
                    {
                        return false;
                    }
                }

                Console.WriteLine($"First date loaded - {firstDate}!");

                Console.Write("Enter second date (yyyy-MM-dd) or 'exit' to go back to menu: ");
                string secondDate = Console.ReadLine()!;

                while (!DateValidator.ValidateDate(secondDate) || firstDate == secondDate)
                {
                    if (firstDate == secondDate)
                    {
                        Console.WriteLine("Second date can not be same as first date");
                    }
                    Console.Write("Enter second date (yyyy-MM-dd) or 'exit' to go back to menu: ");
                    secondDate = Console.ReadLine()!;
                    if (secondDate.ToLower() == "exit")
                    {
                        return false;
                    }
                }

                Console.WriteLine($"Second date loaded - {firstDate}!");

                Console.WriteLine($"Getting exchange rates comparison for {firstDate} - {secondDate}...");

                try
                {
                    IEnumerable<Currency> currencies = SupportedCurrencies.All;
                    IEnumerable<ExchangeRateDifference> rates = await _exchangeRateProvider.CompareExchangeRatesBetweenDates(SupportedCurrencies.All, firstDate, secondDate);

                    if (rates == null || rates.Count() <= 0)
                    {
                        Console.WriteLine($"No exchange rate differences were obtained from {firstDate} to {secondDate}");
                    }
                    else
                    {
                        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rate differences");
                        Console.WriteLine($"from {firstDate} to {secondDate}:");
                        foreach (ExchangeRateDifference rate in rates)
                        {
                            Console.WriteLine(rate.ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Could not retrieve exchange rates: {e.Message}");
                }
                return true;
            }

        }
    }

}
