using System;

namespace ExchangeRateUpdater.Exceptions;

public class ParsingException(string message, Exception innerException)
    : Exception(message, innerException);