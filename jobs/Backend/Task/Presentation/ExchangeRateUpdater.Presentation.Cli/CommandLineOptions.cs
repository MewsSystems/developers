using CommandLine;

namespace ExchangeRateUpdater.Presentation.Cli;

public class CommandLineOptions
{
    [Option('d', "date", Required = false, HelpText = "Specify a date to get exchange rates for")]
    public DateTime? Date { get; set; }
    
    [Option('c', "currencies", Required = true, HelpText = "Comma-separated currencies to get the exchange rates for, at least one is required", Separator = ',', Min = 1)]
    public IEnumerable<string> Currencies { get; set; }
}