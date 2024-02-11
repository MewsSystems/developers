namespace ExchangeRate.Infrastructure.Cnb.Models;

public record CachedValue(DateTime ValidFor, IEnumerable<Models.ExchangeRate> ExchangeRates);
