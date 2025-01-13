using ExchangeRateUpdater.Domain.ValueObjects;
using MediatR;

namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates
{
    public class GetExchangeRatesQuery : IRequest<IEnumerable<ExchangeRate>>
    {
        public required IEnumerable<string> Currencies { get; set; }

        public DateTime? Date { get; set; }
    }
}
