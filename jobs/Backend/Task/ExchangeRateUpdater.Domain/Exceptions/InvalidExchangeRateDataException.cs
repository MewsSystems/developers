namespace ExchangeRateUpdater.Domain.Exceptions;

public sealed class InvalidExchangeRateDataException(string message) : Exception(message);