using ExchangeRateFinder.Application.Models;
using ExchangeRateFinder.Domain.Services;
using ExchangeRateFinder.Infrastructure.Models;
using ExchangeRateFinder.Infrastructure.Services;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application
{
    public interface IExchangeRateService 
    {
        Task<List<CalculatedExchangeRate>> GetExchangeRates(string sourceCurrency, IEnumerable<string> currencies);
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

        public async Task<List<CalculatedExchangeRate>> GetExchangeRates(string sourceCurrency, IEnumerable<string> currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException(nameof(currencies));

            //Do different 
            var calculatedExchangeRates = new List<CalculatedExchangeRate>();

            foreach (var currency in currencies)
            {
                var exchangeRateModel = await _cachingService.GetOrAddAsync(currency, 
                    async () => await _exchangeRateRepository.GetByCodeAsync(currency));

                if(exchangeRateModel != null)
                {
                    var exchangeRate = _exchangeRateCalculator.CalculateExchangeRate(
                                        exchangeRateModel.Amount,
                                        exchangeRateModel.Rate,
                                        sourceCurrency,
                                        exchangeRateModel.Code);

                    calculatedExchangeRates.Add(new CalculatedExchangeRate()
                    {
                        TargetCurrency = exchangeRate.TargetCurrency,
                        SourceCurrency = exchangeRate.SourceCurrency,
                        Value = exchangeRate.Value,
                    });
                }
                else
                {
                    _logger.LogError($"Exchange rate for currency {currencies} was not found.");
                }
            }

            return calculatedExchangeRates;
        }
    }
}
