namespace ExchangeRateUpdater.Interfaces;

public interface IParserFactory
{
    IExchangeRateParser CreateParser(string parserType);
}