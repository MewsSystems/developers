using ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider;

namespace ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler(IExchangeRateProvider exchangeRateProvider) : IRequestHandler<GetExchangeRatesQuery, IEnumerable<GetExchangeRatesResponse>>
    {
        private readonly IExchangeRateProvider _exchangeRateProvider = exchangeRateProvider;

        public async Task<IEnumerable<GetExchangeRatesResponse>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var rates = await _exchangeRateProvider.GetExchangeRates(request.Date);

            if (rates == null)
            {
                return [];
            }

            var result = rates.ToList();

            if (request.IsoCodes is not null && request.IsoCodes.Count != 0)
            {
                var validIsoCodes = request.IsoCodes.Where(iso => !string.IsNullOrWhiteSpace(iso)).Distinct().ToList();
                if (validIsoCodes.Count != 0)
                    return result.Where(rate => validIsoCodes.Contains(rate.CurrencyCode.ToUpper())).Select(rate => new GetExchangeRatesResponse(rate));
                
            }

            return rates.Select(rate => new GetExchangeRatesResponse(rate));
        }
    }
}
