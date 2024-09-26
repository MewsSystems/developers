using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService(IExchangeRateProxy exchangeRateProxy) : IExchangeRateService
    {
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string sourceCurrencyCode, IEnumerable<string> currencyCodes)
        {
            ArgumentNullException.ThrowIfNull(currencyCodes);

            try
            {
                IEnumerable<CurrencyRate> rates = await exchangeRateProxy.GetCurrencyRatesAsync(DateTimeOffset.Now);
                return rates?
                    .Where(x => currencyCodes.Any(c => c == x.CurrencyCode) && x.CurrencyCode != sourceCurrencyCode)
                    .Select(x => new ExchangeRate(new Currency(sourceCurrencyCode), new Currency(x.CurrencyCode), GetExchangeRate(1, x.Rate)))
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
