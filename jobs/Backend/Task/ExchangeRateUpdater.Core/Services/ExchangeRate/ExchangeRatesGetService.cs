using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts.CurrencySource;
using ExchangeRateUpdater.Core.ServiceContracts.ExchangeRate;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Services.ExchangeRate
{
    public class ExchangeRatesGetService : BaseCacheService, IExchangeRateGetService
    {
        private readonly ILogger<ExchangeRatesGetService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly IDistributedCache _distributedCache;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ICurrencySourceGetService _currencySourceGetService;

        public ExchangeRatesGetService(ILogger<ExchangeRatesGetService> logger, IDiagnosticContext diagnosticContext, IDistributedCache distributedCache, IExchangeRateRepository exchangeRateRepository, ICurrencySourceGetService currencySourceGetService)
            :base("ExchangeRate", 10, 2)
        {
            _logger = logger;
            _diagnosticContext = diagnosticContext;
            _distributedCache = distributedCache;
            _exchangeRateRepository = exchangeRateRepository;
            _currencySourceGetService = currencySourceGetService;
        }

        public async Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates()
        {
            _logger.LogInformation("GetExchangeRates of ExchangeRatesGetService called");

            IEnumerable<CurrencySourceResponse> allCurrencySources = await _currencySourceGetService.GetAllCurrencySources();
            var currencySource = allCurrencySources.FirstOrDefault();

            if (currencySource == null) {
                _logger.LogWarning("ExchangeRatesGetService - no currency sources currently stored.");
                throw new ArgumentException("No Currency Sources currently configured.");
            }

            _diagnosticContext.Set("CurrencySource", currencySource);

            string cacheKey = $"{CacheKey}_{currencySource.CurrencyCode}";
            var cachedData = await _distributedCache.GetAsync(cacheKey);
            if (IsCacheEmpty(cachedData))
            {
                var exchangeRates = await _exchangeRateRepository.GetExchangeRatesAsync(currencySource.CurrencyCode, currencySource.SourceUrl);

                _diagnosticContext.Set("AllExchangeRates", exchangeRates);
                _logger.LogInformation("Exchange Rate Repository returned {exchangeRates} results", exchangeRates.Count());

                var rates = exchangeRates.Select(rate => rate.ToExchangeRateResponse()).ToList();

                _logger.LogInformation("ExchangeRatesGetService - GetExchangeRates - Adding exchange rates to cache");
                await _distributedCache.SetAsync(cacheKey, ConvertListToByteArray(rates), CacheOptions);

                return rates;
            }
            else
            {
                _logger.LogInformation("ExchangeRatesGetService - GetExchangeRates - Cached data found.");
                var exchangeRates = ConvertByteArrayToList<ExchangeRateResponse>(cachedData);
                return exchangeRates;
            }
        }

        public async Task<IEnumerable<ExchangeRateResponse>> GetFilteredExchangeRates(List<string> currencyCodes)
        {
            _logger.LogInformation("GetFilteredExchangeRates of ExchangeRatesGetService called with a list of {CurrencyCodes} Currency Codes", currencyCodes.Count());
            _diagnosticContext.Set("CurrencyCodes", currencyCodes);

            IEnumerable<ExchangeRateResponse> exchangeRates = await GetExchangeRates();

            var filteredExchangeRates = exchangeRates.Where(e => currencyCodes.Any(c => e.SourceCurrency.ToUpperInvariant() == c.ToUpperInvariant()));

            _diagnosticContext.Set("FilteredExchangeRates", filteredExchangeRates);
            _logger.LogInformation("Exchange Rate Repository returned {exchangeRates} results", exchangeRates.Count());

            return filteredExchangeRates;
        }
    }
}
