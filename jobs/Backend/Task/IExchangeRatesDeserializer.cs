using System.Collections.Generic;

namespace ExchangeRateUpdater;

public interface IExchangeRatesDeserializer
{
    IEnumerable<ExchangeRate> Deserialize(string serializedExchangeRate);
}