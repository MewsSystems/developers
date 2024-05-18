using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Configuration;
using MediatR;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, GetExchangeRatesQueryResult>
    {
        public GetExchangeRatesQuery GetExchangeRatesQuery { get; set; }

        private readonly IExchangeRateProvider _exchangeRateProvider;

        // service class to get the data
        public GetExchangeRatesQueryHandler(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<GetExchangeRatesQueryResult> Handle(GetExchangeRatesQuery query, CancellationToken cancellationToken) 
        {
            var queriedCurrencies = query.TargetCurrencies;// explicit cast not the best

            var exchangeRates = await _exchangeRateProvider.GetExchangeRates(queriedCurrencies); 

            // construct Result object
            var result = new GetExchangeRatesQueryResult() 
            {
                Message = "Boom!"
            };

            return result;
        }

    }

    //public class GetExchangeRatesRequest
    //{
    //    public IEnumerable<Currency> TargetCurrencies { get; set; }
    //    public DateTime? Date { get; set; } // to set default IF NO DATE SUPPLIED, we get todays date
    //    public string? Lang { get; set; } // if NO lang is supplied, it is CZK
    //}
}
