using MediatR;

namespace ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;

public record GetExchangeRatesByCurrencyQuery(IEnumerable<string> CurrencyCodes, DateTime? ForDate) : IRequest<GetExchangeRatesByCurrencyQueryResponse>;
