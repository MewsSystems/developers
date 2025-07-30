namespace ExchangeRateUpdater.Models;

public class ExchangeRateSources
{
    public string BaseCurrency { get; set; }
    public string Url { get; set; }
    public string ParserType { get; set; }
}