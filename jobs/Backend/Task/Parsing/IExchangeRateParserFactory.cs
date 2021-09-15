namespace ExchangeRateUpdater.Parsing
{
    public interface IExchangeRateParserFactory
    {
        IExchangeRateParser CreateParser(ExchangeRateParserType type);
    }
}
