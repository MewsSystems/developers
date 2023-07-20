using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Components.Queries;

public record GetExchangeRatesQuery(IEnumerable<Currency> Currencies);
