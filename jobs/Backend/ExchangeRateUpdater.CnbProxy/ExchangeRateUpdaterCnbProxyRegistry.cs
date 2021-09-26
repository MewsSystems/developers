using System;
using StructureMap;
using ExchangeRateUpdater.CnbProxy.Services;
using ExchangeRateUpdater.CnbProxy.Configuration;
using ExchangeRateUpdater.CnbProxy.Implementation;

namespace ExchangeRateUpdater.CnbProxy
{
    public sealed class ExchangeRateUpdaterCnbProxyRegistry : Registry
    {
        public ExchangeRateUpdaterCnbProxyRegistry()
        {
            For<ICnbExchangeRatesService>().Use<CnbExchangeRatesService>();
            For<ICnbConfiguration>().Use<CnbConfiguration>();
            For<ICnbHttpClient>().Singleton().Use<CnbHttpClient>();
            For<ICnbXmlDeserializer>().Use<CnbXmlDeserializer>();
        }
    }
}
