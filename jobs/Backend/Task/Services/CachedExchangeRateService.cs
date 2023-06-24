using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Services
{
    public class CachedExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly Func<DateTime, DateTime> _ttlPolicy;

        private (IEnumerable<ExchangeRate> CurrentRates, DateTime TTL) _cachedRates;

        public CachedExchangeRateService(IExchangeRateService exchangeRateService, Func<DateTime, DateTime> ttlPolicy)
        {
            _exchangeRateService = exchangeRateService;
            _ttlPolicy = ttlPolicy;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyExchangeRatesAsync(string targetCurrencyCode)
        {
            if (_cachedRates.CurrentRates == null || DateTime.UtcNow >= _cachedRates.TTL)
            {
                var actualRates = await _exchangeRateService.GetCurrencyExchangeRatesAsync(targetCurrencyCode);
                _cachedRates = (actualRates, _ttlPolicy(DateTime.UtcNow));
            }

            return _cachedRates.CurrentRates;
        }
    }
}
