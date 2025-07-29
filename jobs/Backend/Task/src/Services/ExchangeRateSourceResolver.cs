using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateSourceResolver
{
    private readonly IConfiguration _configuration;
    private readonly IParserFactory _parserFactory;

    public ExchangeRateSourceResolver(IConfiguration configuration, IParserFactory parserFactory)
    {
        _configuration = configuration;
        _parserFactory = parserFactory;
    }

    public (ExchangeRateSource Source, IExchangeRateParser Parser) ResolveSourceAndParser(Currency baseCurrency)
    {
        var sources = _configuration.GetSection("ExchangeRateSources").Get<List<ExchangeRateSource>>();

        var source = sources
            .FirstOrDefault(s => s.BaseCurrency.Equals(baseCurrency.Code, StringComparison.OrdinalIgnoreCase));

        if (source == null)
        {
            throw new InvalidOperationException($"No exchange rate source found for base currency {baseCurrency.Code}");
        }

        var parser = _parserFactory.CreateParser(source.ParserType);

        return (source, parser);
    }   
}