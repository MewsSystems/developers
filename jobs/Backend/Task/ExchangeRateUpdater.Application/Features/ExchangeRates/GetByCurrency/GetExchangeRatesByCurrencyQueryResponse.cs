﻿namespace ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;

public record GetExchangeRatesByCurrencyQueryResponse(IEnumerable<ExchangeRate> ExchangeRates);
