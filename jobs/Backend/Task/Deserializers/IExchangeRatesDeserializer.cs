using System.Collections.Generic;
using ExchangeRateUpdater.DTO;

namespace ExchangeRateUpdater.Deserializers;

public interface IExchangeRatesDeserializer
{
    IEnumerable<ExchangeRate> Deserialize(string serializedExchangeRate);
}