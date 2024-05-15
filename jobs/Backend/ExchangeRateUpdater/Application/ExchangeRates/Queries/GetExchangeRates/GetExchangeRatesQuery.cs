using MediatR;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQuery : IRequest<GetExchangeRatesQueryResult>
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<string> TargetCurrencies { get; set; }
        public decimal RoundingDecimal { get; set; }
    }
}
