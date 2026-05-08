namespace ExchangeRateUpdater.Models;

public class ExchangeRateSources
{
    public string BaseCurrency { get; init; }
    public string Url { get; init; }
    public string ParserType { get; init; }
}