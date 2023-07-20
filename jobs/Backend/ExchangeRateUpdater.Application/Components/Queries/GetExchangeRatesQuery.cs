using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Components.Queries;

public record GetExchangeRatesQuery(NonEmptyList<Currency> Currencies);
