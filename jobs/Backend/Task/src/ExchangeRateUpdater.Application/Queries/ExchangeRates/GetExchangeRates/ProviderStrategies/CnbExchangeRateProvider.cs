
using ExchangeRateUpdater.Application.Const;
using ExchangeRateUpdater.Application.Contracts.Caching;
using ExchangeRateUpdater.Application.Contracts.Persistence;
using ExchangeRateUpdater.Domain.Const;
using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies
{
    /// <summary>
    /// Concrete strategy for provider Czech National Bank
    /// </summary>
    public class CnbExchangeRateProvider : ExchangeRateProviderStrategyBase
    {
        protected override string ProviderCode => ProviderConstants.CnbProviderCode;

        private readonly ICnbExchangeRateRepository _cnbExchangeRateRepository;
        private readonly ICacheService _cache;

        public CnbExchangeRateProvider(ICnbExchangeRateRepository cnbExchangeRateRepository, ICacheService cache)
        {
            _cnbExchangeRateRepository = cnbExchangeRateRepository;
            _cache = cache;
        }

        public override async Task<IEnumerable<GetExchangeRatesQueryResponse>> GetExchangeRatesAsync(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ExchangeRate> exRates = [];
            var exRateDate = request.Date ?? DateTime.Today;
           
            var cacheKey = CacheConstants.ExchangeRateKey(request.ProviderCode, exRateDate);
            if (!_cache.Exists(cacheKey))
            {
                exRates = await _cnbExchangeRateRepository.GetExchangeRatesAsync(exRateDate, cancellationToken);
                _cache.Add(cacheKey, exRates);
            }

            exRates = _cache.Get<IEnumerable<ExchangeRate>>(cacheKey);

            var exRatesFilteredByCurrencies = exRates.Where(e => request.Currencies.Any(c => c == e.SourceCurrency.Code));

            return exRatesFilteredByCurrencies.Select(x =>
                new GetExchangeRatesQueryResponse()
                {
                    SourceCurrency = x.SourceCurrency.Code,
                    TargetCurrency = x.TargetCurrency.Code,
                    Value = x.Value,
                    ValidFor = exRateDate
                });
        }
    }
}
