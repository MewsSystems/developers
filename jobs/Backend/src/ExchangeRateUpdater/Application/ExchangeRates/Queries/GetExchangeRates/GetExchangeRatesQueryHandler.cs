using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Interfaces;
using MediatR;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, GetExchangeRatesQueryResult>
    {
        public GetExchangeRatesQuery GetExchangeRatesQuery { get; set; }
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public GetExchangeRatesQueryHandler(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<GetExchangeRatesQueryResult> Handle(GetExchangeRatesQuery query, CancellationToken cancellationToken) 
        {
            var exchangeRates = await _exchangeRateProvider.GetExchangeRates(query.Currencies);

            return new GetExchangeRatesQueryResult() 
            {
                ExchangeRates = exchangeRates
            };
        }
    }
}
