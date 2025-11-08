using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateProviders.Tests.TestHelpers;

public static class LogVerificationExtensions
{
    /// <summary>
    /// Verifies that a logger received a log message with the specified level and exact message content.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="level">The expected log level</param>
    /// <param name="message">The exact message content expected</param>
    public static void VerifyLog<T>(this ILogger<T> logger, int times, LogLevel level, string message)
    {
        logger.Received(times).Log(
            level,
            Arg.Any<EventId>(),
            Arg.Is<object>(v => v.ToString() == message),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// Verifies that a logger received a Debug level log message with the specified exact content.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="message">The exact message content expected</param>
    public static void VerifyLogDebug<T>(this ILogger<T> logger, int times, string message)
    {
        logger.VerifyLog(times, LogLevel.Debug, message);
    }

    /// <summary>
    /// Verifies that a logger received an Information level log message with the specified exact content.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="message">The exact message content expected</param>
    public static void VerifyLogInformation<T>(this ILogger<T> logger, int times, string message)
    {
        logger.VerifyLog(times, LogLevel.Information, message);
    }

    /// <summary>
    /// Verifies that a logger received a Warning level log message with the specified exact content.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="message">The exact message content expected</param>
    public static void VerifyLogWarning<T>(this ILogger<T> logger, int times, string message)
    {
        logger.VerifyLog(times, LogLevel.Warning, message);
    }

    /// <summary>
    /// Verifies that a logger received an Error level log message with the specified exact content.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="message">The exact message content expected</param>
    public static void VerifyLogError<T>(this ILogger<T> logger, int times, string message)
    {
        logger.VerifyLog(times, LogLevel.Error, message);
    }

    /// <summary>
    /// Verifies that a logger did NOT receive any log message containing the specified text.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="level">The log level to check</param>
    /// <param name="messageContains">Text that should NOT appear in any log message</param>
    public static void VerifyLogNotContaining<T>(this ILogger<T> logger, LogLevel level, string messageContains)
    {
        logger.DidNotReceive().Log(
            level,
            Arg.Any<EventId>(),
            Arg.Is<object>(v => v.ToString()!.Contains(messageContains)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    /// <summary>
    /// Verifies that a logger did NOT receive any Information level log message containing the specified text.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="messageContains">Text that should NOT appear in any Information log message</param>
    public static void VerifyLogInformationNotContaining<T>(this ILogger<T> logger, string messageContains)
    {
        logger.VerifyLogNotContaining(LogLevel.Information, messageContains);
    }

    /// <summary>
    /// Verifies that a logger did NOT receive any Warning level log message containing the specified text.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="messageContains">Text that should NOT appear in any Warning log message</param>
    public static void VerifyLogWarningNotContaining<T>(this ILogger<T> logger, string messageContains)
    {
        logger.VerifyLogNotContaining(LogLevel.Warning, messageContains);
    }
}