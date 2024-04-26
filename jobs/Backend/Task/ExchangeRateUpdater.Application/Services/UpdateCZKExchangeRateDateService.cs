﻿using ExchangeRateFinder.Application.Configuration;
using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateFinder.Application
{
    public interface IUpdateCZKExchangeRateDataService
    {
        Task UpdateDataAsync();
    }

    public class UpdateCZKExchangeRateDataService : IUpdateCZKExchangeRateDataService
    {
        private readonly IWebDataFetcher _webDataFetcher;
        private readonly IExchangeRateParser _exchangeRateParser;
        private readonly ICachingService<ExchangeRate> _cachingService;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ILogger<UpdateCZKExchangeRateDataService> _logger;
        private readonly CBNConfiguration _options;
        private readonly string SourceCurrency = "CZK";

        public UpdateCZKExchangeRateDataService(
            IWebDataFetcher webDataFetcher, 
            IExchangeRateParser exchangeRateParser, 
            ICachingService<ExchangeRate> cacheService, 
            IExchangeRateRepository exchangeRateRepository,
            IOptions<CBNConfiguration> options,
            ILogger<UpdateCZKExchangeRateDataService> logger)
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
            try
            {
                _logger.LogInformation($"Updating of CZK exchange rate data has started at {DateTime.Now}");

                var exchangeRateData = await _webDataFetcher.GetDataFromUrl(_options.Url);
                // Parse it 
                var exchangeRates = _exchangeRateParser.Parse(SourceCurrency, exchangeRateData);

                // Update the database
                await _exchangeRateRepository.UpdateAllAsync(SourceCurrency, exchangeRates);

                // Update the cache
                var exchangeRatesForCache = exchangeRates.ToDictionary(x => $"{SourceCurrency}-{x.Code}", x => x);
                _cachingService.UpdateCache(exchangeRatesForCache);

                _logger.LogInformation("Updating of CZK exchange rate data has finished.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Updating of CZK exchange rate data has throw error:{ex.Message}");
            }
        }
    }
}
