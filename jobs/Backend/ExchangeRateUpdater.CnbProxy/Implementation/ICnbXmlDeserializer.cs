using System;

namespace ExchangeRateUpdater.CnbProxy.Implementation
{
    interface ICnbXmlDeserializer
    {
        T Deserialize<T>(string xmlContent) where T : class;
    }
}
