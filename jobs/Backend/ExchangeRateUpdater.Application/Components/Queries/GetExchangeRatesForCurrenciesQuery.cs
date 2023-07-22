using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Components.Queries;

public record GetExchangeRatesForCurrenciesQuery(NonEmptyList<Currency> Currencies);
