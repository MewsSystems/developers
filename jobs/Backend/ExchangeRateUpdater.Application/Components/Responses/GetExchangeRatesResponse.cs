using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Components.Responses;

public record GetExchangeRatesResponse(IEnumerable<ExchangeRate> ExchangeRates);