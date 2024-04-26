using ExchangeRateFinder.Application.Configuration;
using ExchangeRateFinder.Infrastructure.Caching;
using ExchangeRateFinder.Infrastructure.Interfaces;
using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateFinder.Application
{
    public interface IUpdateExchangeRateDateService
    {
        Task UpdateDataAsync();
    }

    public class UpdateExchangeRateDateService : IUpdateExchangeRateDateService
    {
        private readonly IWebDataFetcher _webDataFetcher;
        private readonly IExchangeRateParser _exchangeRateParser;
        private readonly ICachingService<ExchangeRate> _cachingService;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ILogger<UpdateExchangeRateDateService> _logger;
        private readonly CBNConfiguration _options;

        public UpdateExchangeRateDateService(
            IWebDataFetcher webDataFetcher, 
            IExchangeRateParser exchangeRateParser, 
            ICachingService<ExchangeRate> cacheService, 
            IExchangeRateRepository exchangeRateRepository,
            IOptions<CBNConfiguration> options,
            ILogger<UpdateExchangeRateDateService> logger)
        {
            _webDataFetcher = webDataFetcher;
            _exchangeRateParser = exchangeRateParser;
            _cachingService = cacheService;
            _exchangeRateRepository = exchangeRateRepository;
            _logger = logger;
            _options = options.Value;
        }

        public async Task UpdateDataAsync()
        {
            // Get the data from source 
            var exchangeRateData = await _webDataFetcher.GetDataFromUrl(_options.Url);
            // Parse it 
            var exchangeRates = _exchangeRateParser.Parse("CZK", exchangeRateData);

            // Update the database
            await _exchangeRateRepository.UpdateAllAsync(exchangeRates);

            // Update the cache
            var exchangeRatesForCache = exchangeRates.ToDictionary(x => x.Code, x => x);
            _cachingService.UpdateCache(exchangeRatesForCache);
        }
    }
}
