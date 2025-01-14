using ExchangeRateUpdater.Domain.Const;
using MediatR;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates
{
    public class GetExchangeRatesQuery : IRequest<IEnumerable<GetExchangeRatesQueryResponse>>
    {
        public required IEnumerable<string> Currencies { get; set; }

        public DateTime? Date { get; set; }

        public string ProviderCode { get; set; } = ProviderConstants.CnbProviderCode;
    }
}
