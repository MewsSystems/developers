using ExchangeRates.Domain.Entities;
using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRates;

public record GetExchangeRatesQuery(DateTime? Day) : IRequest<IEnumerable<ExchangeRate>>;
