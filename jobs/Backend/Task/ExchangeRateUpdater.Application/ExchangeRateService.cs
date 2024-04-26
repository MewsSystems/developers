using ExchangeRateFinder.Domain.Services;
using ExchangeRateFinder.Infrastructure.Caching;
using ExchangeRateFinder.Infrastructure.Interfaces;
using ExchangeRateFinder.Infrastructure.Models;

namespace ExchangeRateUpdater.Application
{
    public interface IExchangeRateService 
    {
        Task<List<ExchangeRateViewModel>> GetExchangeRates(string sourceCurrency, IEnumerable<string> currencies);
    }
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ICachingService<ExchangeRate> _cachingService;
        private readonly IExchangeRateCalculator _exchangeRateCalculator;
        private const string SourceCurrencyCode = "CZK";

        public ExchangeRateService(
            IExchangeRateRepository exchangeRateRepository,
            ICachingService<ExchangeRate> cachingService,
            IExchangeRateCalculator exchangeRateCalculator)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _cachingService = cachingService;
            _exchangeRateCalculator = exchangeRateCalculator;
        }

        public async Task<List<ExchangeRateViewModel>> GetExchangeRates(string sourceCurrency, IEnumerable<string> currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException(nameof(currencies));

            var exchangeRates = new List<ExchangeRateViewModel>();

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

                    exchangeRates.Add(new ExchangeRateViewModel()
                    {
                        TargetCurrency = exchangeRate.TargetCurrency,
                        SourceCurrency = exchangeRate.SourceCurrency,
                        Value = exchangeRate.Value,
                    });
                }
                else
                {
                    // Skip (log)
                }
            }

            return exchangeRates;
        }
    }
}
