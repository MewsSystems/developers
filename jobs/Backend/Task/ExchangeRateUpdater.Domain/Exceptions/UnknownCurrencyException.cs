namespace ExchangeRateUpdater.Domain.Exceptions;

public sealed class UnknownCurrencyException(string code) : Exception($"Unknown currency code: {code}");