
using MediatR;
using ErrorOr;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetDailyExchangeRates
{
    public record GetDailyExchangeRatesQuery() : IRequest<ErrorOr<IEnumerable<ExchangeRate>>>;
}
