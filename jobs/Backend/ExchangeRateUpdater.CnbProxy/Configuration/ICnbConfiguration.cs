using System;

namespace ExchangeRateUpdater.CnbProxy.Configuration
{
    public interface ICnbConfiguration
    {
        string UrlToXmlExchangeRates { get; }
    }
}
