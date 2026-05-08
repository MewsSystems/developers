using System;

namespace ExchangeRateUpdater.DataSources;

public class RateLoaderException(string message, Exception inner) : Exception(message, inner)
{
}
