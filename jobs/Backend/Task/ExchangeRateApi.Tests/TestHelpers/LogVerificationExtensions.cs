using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateApi.Tests.TestHelpers;

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
    /// Verifies that a logger received a log message with the specified level and message containing the specified text.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="level">The expected log level</param>
    /// <param name="messageContains">Text that should be contained in the log message</param>
    /// <param name="exception">The expected exception (optional)</param>
    public static void VerifyLogContaining<T>(this ILogger<T> logger, int times, LogLevel level, string messageContains, Exception? exception = null)
    {
        if (exception != null)
        {
            logger.Received(times).Log(
                level,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString()!.Contains(messageContains)),
                exception,
                Arg.Any<Func<object, Exception?, string>>());
        }
        else
        {
            logger.Received(times).Log(
                level,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString()!.Contains(messageContains)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
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
    /// Verifies that a logger received an Error level log message containing the specified text.
    /// </summary>
    /// <typeparam name="T">The logger category type</typeparam>
    /// <param name="logger">The substitute logger instance</param>
    /// <param name="times">Expected number of times the log should be called</param>
    /// <param name="messageContains">Text that should be contained in the error log message</param>
    /// <param name="exception">The expected exception (optional)</param>
    public static void VerifyLogErrorContaining<T>(this ILogger<T> logger, int times, string messageContains, Exception? exception = null)
    {
        logger.VerifyLogContaining(times, LogLevel.Error, messageContains, exception);
    }
}