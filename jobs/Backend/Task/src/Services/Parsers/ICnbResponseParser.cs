namespace ExchangeRateUpdater;

public interface ICnbResponseParser
{
    Dictionary<string, decimal> Parse(string xmlContent, DateTime requestedDate);
}