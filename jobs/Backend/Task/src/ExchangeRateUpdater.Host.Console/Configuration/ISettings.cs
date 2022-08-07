using Serilog.Events;

namespace ExchangeRateUpdater.Host.Console.Configuration;

internal interface ISettings
{
    string HttpLogEndpoint { get; }
    LogEventLevel MinimumLogLevel { get; }
}