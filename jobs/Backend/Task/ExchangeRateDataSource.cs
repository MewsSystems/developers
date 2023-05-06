using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateDataSource : IExchangeRateDataSource
    {
        private readonly HttpClient httpClient;
        private readonly IExchangeRateDataSourceOptions options;

        public ExchangeRateDataSource(IExchangeRateDataSourceOptions options, HttpClient httpClient)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();

            try
            {
                var dailyRateTask = GetDailyRatesAsync();
                var monthlyRateTask = GetMonthlyRatesAsync();

                await Task.WhenAll(dailyRateTask, monthlyRateTask);

                foreach (var currency in currencies)
                {
                    var dailyRates = dailyRateTask.Result.Where(r => r.SourceCurrency.Code == currency.Code);
                    var monthlyRates = monthlyRateTask.Result.Where(r => r.SourceCurrency.Code == currency.Code);

                    rates.AddRange(dailyRates);
                    rates.AddRange(monthlyRates);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the exchange rates: {ex.Message}");
            }

            return rates;
        }

        private async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync()
        {
            var rates = new List<ExchangeRate>();

            try
            {
                var response = await httpClient.GetAsync($"{options.DailyRatesUrl}?date={DateTime.Today:dd.MM.yyyy}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                rates.AddRange(ParseExchangeRates(content, Currency.CZK));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the daily exchange rates: {ex.Message}");
            }

            return rates;
        }

        private async Task<IEnumerable<ExchangeRate>> GetMonthlyRatesAsync()
        {
            var rates = new List<ExchangeRate>();

            try
            {
                var response = await httpClient.GetAsync($"{options.MonthlyRatesUrl}?year={DateTime.Today.Year}&month={DateTime.Today.Month - 1}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                rates.AddRange(ParseExchangeRates(content, Currency.CZK));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the monthly exchange rates: {ex.Message}");
            }

            return rates;
        }

        private List<ExchangeRate> ParseExchangeRates(string content, Currency targetCurrency)
        {
            var exchangeRates = new List<ExchangeRate>();

            var lines = content.Split('\n');

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('|');

                if (parts.Length >= 5)
                {
                    var sourceAmount = parts[2];
                    var sourceCode = parts[3];
                    var sourceRate = parts[4];

                    decimal sourceAmountDecimal, sourceRateDecimal;
                    if (decimal.TryParse(sourceAmount, out sourceAmountDecimal) && decimal.TryParse(sourceRate, out sourceRateDecimal))
                    {
                        var calculatedRate = sourceRateDecimal / sourceAmountDecimal;
                        var exchangeRate = new ExchangeRate(new Currency(sourceCode), new Currency(targetCurrency.Code), calculatedRate);
                        exchangeRates.Add(exchangeRate);
                    }
                    //else
                    //{
                    //    Console.WriteLine("Invalid source amount or rate value");
                    //}
                }
            }

            return exchangeRates;
        }
    }
}

