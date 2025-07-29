using ExchangeRateUpdater.Interfaces;
using System;

namespace ExchangeRateUpdater.Parsers;

public class ParserFactory : IParserFactory
{
    public IExchangeRateParser CreateParser(string parserType)
    {
        return parserType switch
        {
            "CnbXmlParser" => new CnbXmlParser(),
            _ => throw new InvalidOperationException($"Unknown parser type: {parserType}"),
        };
    }
}