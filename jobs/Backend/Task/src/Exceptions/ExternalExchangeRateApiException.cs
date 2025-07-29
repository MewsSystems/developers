using System;

namespace ExchangeRateUpdater.Exceptions;

public class ExternalExchangeRateApiException(string message, Exception innerException)
    : Exception(message, innerException);