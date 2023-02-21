using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Sources;
using System;

namespace ExchangeRateUpdater.ExchangeRateSources.CNB;

public static class CNBSourceFactory
{
    public static ISource CreateSource(CNBSourceOptions options) =>
        options.Location switch
        {
            SourceLocation.URL => new HttpSource(options.DayRateUrl),
            SourceLocation.File => new FileSource(options.FileUri),
            _ => throw new NotSupportedException($"Location type is not supported {options.Location}")
        };
}
