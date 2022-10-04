using System;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

internal class ExchangeRateLoaderFactory
{
    public IExchangeRateLoader Create()
    {
        // Here you could load CNB URL for example from file.
        // But let's keep it simple for now

        return new ExchangeRateLoader(new Uri(
            "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt"));
    }
}