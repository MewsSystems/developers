using System;

namespace ExchangeRateUpdater.Abstractions.Exceptions;

public class ExchangeRateNotFoundException(string message) : Exception(message);

