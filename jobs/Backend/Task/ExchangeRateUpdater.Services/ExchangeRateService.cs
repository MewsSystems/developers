using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Service for retrieving exchange rates using a specified provider.
    /// </summary>
    /// <param name="provider"></param>
    public class ExchangeRateService(IExchangeRateProvider provider) : IExchangeRatesService
    {
        private readonly IExchangeRateProvider provider = provider ?? throw new ArgumentNullException(nameof(provider));

        public async Task<IReadOnlyList<ExchangeRate>> GetRates(IList<string> currencyCodes)
        {
            if (currencyCodes == null || !currencyCodes.Any())
            {
                throw new ArgumentException("Currency codes cannot be empty.");
            }

            var currencies = currencyCodes
                .Select(code => new Currency(code.Trim().ToUpperInvariant()))
                .ToArray();
            
            return (await provider.GetExchangeRates(currencies)).ToList();
        }
    }
}