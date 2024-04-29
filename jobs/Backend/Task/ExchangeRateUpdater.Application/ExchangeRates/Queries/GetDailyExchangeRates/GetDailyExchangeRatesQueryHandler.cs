using ErrorOr;
using MediatR;
using System.Globalization;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetDailyExchangeRates
{
    public class GetDailyExchangeRatesQueryHandler : IRequestHandler<GetDailyExchangeRatesQuery, ErrorOr<IEnumerable<ExchangeRate>>>
    {
        private readonly IExchangeRateProviderRepository _exchangeRateProviderRepository;
        
        public GetDailyExchangeRatesQueryHandler(IExchangeRateProviderRepository exchangeRateProviderRepository)
        {
            _exchangeRateProviderRepository = exchangeRateProviderRepository;
        }
        public async Task<ErrorOr<IEnumerable<ExchangeRate>>> Handle(GetDailyExchangeRatesQuery query, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var responseOrError = await _exchangeRateProviderRepository.GetCentralBankRates(currentDate, cancellationToken);

            return responseOrError;
        }
    }
}
