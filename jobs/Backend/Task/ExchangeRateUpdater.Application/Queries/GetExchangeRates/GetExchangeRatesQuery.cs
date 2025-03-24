using ExchangeRateUpdater.Core.Models;
using MediatR;

namespace ExchangeRateUpdater.Application.Queries.GetExchangeRates;

public record GetExchangeRatesQuery(IEnumerable<Currency> Currencies, DateTime? Date = null) : IRequest<IList<ExchangeRate>>; 