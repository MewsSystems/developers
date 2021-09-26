using System;
using ExchangeRateUpdater.Utilities.Logging;
using StructureMap;

namespace ExchangeRateUpdater.Utilities
{
    public sealed class ExchangeRateUpdaterUtilitiesRegistry : Registry
    {
        public ExchangeRateUpdaterUtilitiesRegistry()
        {
            For<IAppLogger>().Singleton().Use<AppLogger>();
        }
    }
}
