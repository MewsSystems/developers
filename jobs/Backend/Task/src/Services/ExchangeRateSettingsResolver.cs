using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Parsers;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateSettingsResolver : IExchangeRateSettingsResolver
{
    private readonly IConfiguration _configuration;

    public ExchangeRateSettingsResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ExchangeRateSettings ResolveSourceSettings(Currency baseCurrency)
    {
        var sources = _configuration.GetSection("ExchangeRateSources").Get<List<ExchangeRateSources>>();

        var source = sources
            .FirstOrDefault(s => s.BaseCurrency.Equals(baseCurrency.Code, StringComparison.OrdinalIgnoreCase));

        if (source == null)
        {
            throw new InvalidOperationException($"No exchange rate source found for base currency {baseCurrency.Code}");
        }

        var parser = ParserFactory.CreateParser(source.ParserType);

        return new ExchangeRateSettings(source.Url, parser);
    }   
}