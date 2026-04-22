using System;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Infrastructure;

public sealed record CacheSnapshot(
    ExchangeRate[] Rates,
    DateTimeOffset CachedAt,
    DateTimeOffset? LastModified);
