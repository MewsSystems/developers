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
        Task<List<CalculatedExchangeRate>> GetExchangeRates(string sourceCurrency, IEnumerable<string> currencyCodes, CancellationToken cancellationToken = default);
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

        public async Task<List<CalculatedExchangeRate>> GetExchangeRates(string sourceCurrency, IEnumerable<string> currencyCodes, CancellationToken cancellationToken = default)
        {
            if (currencyCodes == null || string.IsNullOrEmpty(sourceCurrency))
                throw new ArgumentNullException(nameof(currencyCodes));

            var calculatedExchangeRates = new List<CalculatedExchangeRate>();

            foreach (var currencyCode in currencyCodes)
            {
                var exchangeRateModel = await _cachingService.GetOrAddAsync($"{sourceCurrency}-{currencyCode}", 
                    async () => await _exchangeRateRepository.GetAsync(currencyCode, sourceCurrency, cancellationToken));

                if(exchangeRateModel != null)
                {
                    var exchangeRate = _exchangeRateCalculator.Calculate(
                                        exchangeRateModel.Amount,
                                        exchangeRateModel.Rate,
                                        sourceCurrency,
                                        exchangeRateModel.CurrencyCode);

                    calculatedExchangeRates.Add(new CalculatedExchangeRate()
                    {
                        TargetCurrency = exchangeRate.TargetCurrency,
                        SourceCurrency = exchangeRate.SourceCurrency,
                        Value = exchangeRate.Value,
                    });
                }
                else
                {
                    _logger.LogWarning($"Exchange rate for currency {currencyCode} was not found.");
                }
            }

            return calculatedExchangeRates;
        }
    }
}
