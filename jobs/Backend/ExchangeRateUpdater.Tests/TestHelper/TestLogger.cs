using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.Tests.Services.TestHelper
{
    public class TestLogger<T> : ILogger<T>
    {
        public List<LogMessage> LogMessages { get; }

        public TestLogger()
        {
            LogMessages = new List<LogMessage>();
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            LogMessages.Add(new LogMessage(logLevel, formatter(state, exception)));
        }

        public void ClearLogs()
        {
            LogMessages.Clear();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state)
            => default;

        public bool ContainsLog(LogLevel logLevel, string message) =>
            LogMessages.Any(m => m.LogLevel == logLevel && m.Message == message);

        public bool ContainsLogMatchingRegex(LogLevel logLevel, string regex) =>
            LogMessages.Any(m => m.LogLevel == logLevel && Regex.IsMatch(m.Message, regex));
    }

    public class LogMessage
    {
        public LogLevel LogLevel { get; }
        public string Message { get; }

        public LogMessage(LogLevel logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }
    }
}

