using ExchangeRateUpdater.Application.Contracts.Persistence;
using ExchangeRateUpdater.Domain.ValueObjects;
using MediatR;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, IEnumerable<ExchangeRate>>
    {
        private readonly ICnbExchangeRateRepository _cnbExchangeRateRepository;

        public GetExchangeRatesQueryHandler(ICnbExchangeRateRepository cnbExchangeRateRepository)
        {
            _cnbExchangeRateRepository = cnbExchangeRateRepository;
        }

        public async Task<IEnumerable<ExchangeRate>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var exRateDate = request.Date ?? DateTime.Today;

            var dateExRates = await _cnbExchangeRateRepository.GetExchangeRatesAsync(exRateDate, cancellationToken);

            return dateExRates.Where(e => request.Currencies.Any(c => c == e.SourceCurrency.Code));
        }
    }
}
