using ExchangeRateUpdater.Application.Models;
using MediatR;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQuery : IRequest<GetExchangeRatesQueryResult>
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<string> Currencies { get; set; }
        public decimal RoundingDecimal { get; set; }
    }
}
