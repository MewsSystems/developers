using ExchangeRateFinder.Domain.Entities;
using ExchangeRateFinder.Domain.Services;
using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application
{
    public interface IExchangeRateService 
    {
        Task<List<CalculatedExchangeRate>> GetExchangeRates(string sourceCurrencyCode, IEnumerable<string> targetCurrencyCodes, CancellationToken cancellationToken = default);
    }
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ICachingService<ExchangeRate> _cachingService;
        private readonly IExchangeRateCalculator _exchangeRateCalculator;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            ICachingService<ExchangeRate> cachingService,
            IExchangeRateCalculator exchangeRateCalculator,
            ILogger<ExchangeRateService> logger)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _cachingService = cachingService;
            _exchangeRateCalculator = exchangeRateCalculator;
            _logger = logger;
        }

        public async Task<List<CalculatedExchangeRate>> GetExchangeRates(string sourceCurrencyCode, IEnumerable<string> targetCurrencyCodes, CancellationToken cancellationToken = default)
        {
            var calculatedExchangeRates = new List<CalculatedExchangeRate>();

            foreach (var targetCurrencyCode in targetCurrencyCodes)
            {
                var exchangeRateModel = await _cachingService.GetOrAddAsync($"{targetCurrencyCode}-{sourceCurrencyCode}", 
                    async () => await _exchangeRateRepository.GetAsync(targetCurrencyCode, sourceCurrencyCode, cancellationToken));

                if(exchangeRateModel != null)
                {
                    var exchangeRate = _exchangeRateCalculator.Calculate(
                                        exchangeRateModel.Amount,
                                        exchangeRateModel.Value,
                                        sourceCurrencyCode,
                                        exchangeRateModel.TargetCurrencyCode);

                    calculatedExchangeRates.Add(new CalculatedExchangeRate()
                    {
                        TargetCurrencyCode = exchangeRate.TargetCurrencyCode,
                        SourceCurrencyCode = exchangeRate.SourceCurrencyCode,
                        Rate = exchangeRate.Rate,
                    });
                }
                else
                {
                    _logger.LogWarning($"Exchange rate for currency {targetCurrencyCode} was not found.");
                }
            }

            return calculatedExchangeRates;
        }
    }
}
