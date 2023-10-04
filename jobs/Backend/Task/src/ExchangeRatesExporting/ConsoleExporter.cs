using ExchangeRatesUpdater.Common;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesExporting;

public interface IConsoleExporter : IExporter { }

public class ConsoleExporter : IConsoleExporter
{
    private readonly ILogger<ConsoleExporter> logger;

    public ConsoleExporter(ILogger<ConsoleExporter> logger)
    {
        this.logger = logger;
    }

    public void Export(IEnumerable<ExchangeRate> exchangeRates)
    {
        foreach (ExchangeRate exchangeRate in exchangeRates) {
            try {
                Console.WriteLine(exchangeRate.ToString());
            } catch (Exception e) {
                logger.LogError(e, "Could not export exchange rate.");
                return;
            }
        }
    }
}
