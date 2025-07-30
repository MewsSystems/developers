using System;

namespace ExchangeRateUpdater.Exceptions;

public class ExchangeRateApiException(string message, Exception innerException)
    : Exception(message, innerException);