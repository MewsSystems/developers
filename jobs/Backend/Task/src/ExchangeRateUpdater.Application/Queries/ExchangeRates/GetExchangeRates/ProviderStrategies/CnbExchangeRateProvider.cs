
using ExchangeRateUpdater.Application.Contracts.Persistence;
using ExchangeRateUpdater.Domain.Const;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies
{
    /// <summary>
    /// Concrete strategy for provider Czech National Bank
    /// </summary>
    public class CnbExchangeRateProvider : ExchangeRateProviderStrategyBase
    {
        protected override string ProviderCode => ProviderConstants.CnbProviderCode;

        private readonly ICnbExchangeRateRepository _cnbExchangeRateRepository;

        public CnbExchangeRateProvider(ICnbExchangeRateRepository cnbExchangeRateRepository)
        {
            _cnbExchangeRateRepository = cnbExchangeRateRepository;
        }

        public override async Task<IEnumerable<GetExchangeRatesQueryResponse>> GetExchangeRatesAsync(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var exRateDate = request.Date ?? DateTime.Today;

            var exRates = await _cnbExchangeRateRepository.GetExchangeRatesAsync(exRateDate, cancellationToken);

            var exRatesFilteredByCurrencies = exRates.Where(e => request.Currencies.Any(c => c == e.SourceCurrency.Code));

            return exRatesFilteredByCurrencies.Select(x =>
                new GetExchangeRatesQueryResponse()
                {
                    SourceCurrency = x.SourceCurrency.Code,
                    TargetCurrency = x.TargetCurrency.Code,
                    Value = x.Value
                });
        }
    }
}
