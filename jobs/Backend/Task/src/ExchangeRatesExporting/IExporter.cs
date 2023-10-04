using ExchangeRatesUpdater.Common;

namespace ExchangeRatesExporting;

public interface IExporter
{
    void Export(IEnumerable<ExchangeRate> exchangeRates);
}
