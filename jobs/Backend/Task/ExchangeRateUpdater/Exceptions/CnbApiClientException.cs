using System;

namespace ExchangeRateUpdater.Exceptions;

public class CnbApiClientException(string message, Exception innerException) : Exception(message, innerException);