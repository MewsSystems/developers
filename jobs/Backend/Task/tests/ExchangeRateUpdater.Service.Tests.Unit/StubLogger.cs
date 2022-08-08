using Serilog;
using Serilog.Events;

namespace ExchangeRateUpdater.Service.Tests.Unit;

internal class StubLogger : ILogger
{
    public void Write(LogEvent logEvent)
    {
    }

    public void Debug(string messageTemplate, params object[] propertyValues)
    {
    }

    public void Information(string messageTemplate, params object[] propertyValues)
    {
    }

    public void Warning(string messageTemplate, params object[] propertyValues)
    {
    }

    public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
    {
    }

    public void Error(string messageTemplate, params object[] propertyValues)
    {
    }

    public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
    {
    }

    public void Fatal(string messageTemplate, params object[] propertyValues)
    {
    }

    public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
    {
    }

    public ILogger ForContext(string propertyName, object value)
    {
        return this;
    }
}