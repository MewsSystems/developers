using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater;

public class CzechNationalBankExchangeRatesDeserializer : IExchangeRatesDeserializer
{
    private readonly IExchangeRateDeserializer _deserializer;

    public CzechNationalBankExchangeRatesDeserializer(IExchangeRateDeserializer deserializer)
    {
        _deserializer = deserializer;
    }

    public IEnumerable<ExchangeRate> Deserialize(string serializedExchangeRate)
    {
        if (string.IsNullOrEmpty(serializedExchangeRate))
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        return serializedExchangeRate
            .Split(new []{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
            .Skip(2)
            .Select(_deserializer.Deserialize);
    }
}