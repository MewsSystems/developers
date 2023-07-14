using ExchangeRateUpdater.Interface.DTOs;
using MediatR;

namespace ExchangeRateUpdater.Implementation.Queries
{
    public class GetExchangeRatesQuery : IRequest<IEnumerable<ExchangeRateDto>>
    {
        public IEnumerable<CurrencyDto> Currencies { get; set; }
    }
}
