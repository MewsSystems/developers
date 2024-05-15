using MediatR;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, GetExchangeRatesQueryResult>
    {
        // service class to get the data
        public GetExchangeRatesQueryHandler()
        {

        }

        public Task<GetExchangeRatesQueryResult> Handle(GetExchangeRatesQuery query, CancellationToken cancellationToken) 
        {
            // use service class to get the data

            // construct Result object
            var result = new GetExchangeRatesQueryResult() 
            {
                Message = "Boom!"
            };

            return Task.FromResult(result);
        }

    }
}
