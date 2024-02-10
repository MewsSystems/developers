using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService(ICzechNationalBankClient apiClient) : IExchangeRateService
    {
        private readonly ICzechNationalBankClient _apiClient = apiClient;

        public async Task<IEnumerable<IExchangeRate>> GetExchangeRatesAsync(string sourceCurrencyCode, IEnumerable<ICurrency> currencies)
        {
            ArgumentNullException.ThrowIfNull(currencies);

            try
            {
                ExRateDailyResponse result = await _apiClient.CnbapiExratesDailyAsync(DateTimeOffset.Now, Lang.EN);
                return result.Rates?
                    .Where(x => currencies.Any(c => c.Code == x.CurrencyCode) && x.CurrencyCode != sourceCurrencyCode)
                    .Select(x => new ExchangeRate(new Currency(sourceCurrencyCode), new Currency(x.CurrencyCode), GetExchangeRate(1, (decimal)x.Rate)))
                    .ToList();
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Api Exception: {ex.Message}");
                throw;
            }
        }

        private static decimal GetExchangeRate(decimal sourceAmount, decimal targetRate) => Math.Round(sourceAmount / targetRate, 2);
    }
}
