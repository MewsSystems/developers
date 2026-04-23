using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRate.UnitTests.Extensions;

public static class LoggerVerificationExtensions
{
    public static void VerifyLog<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel logLevel,
        string message,
        int? eventId = null,
        Func<Exception, bool> exceptionPredicate = null
    )
    {
        loggerMock.Verify(logger =>
            logger.Log(
                logLevel,
                It.Is<EventId>(e => !eventId.HasValue || eventId.Value == e.Id),
                It.Is<It.IsAnyType>((value, _) => value.ToString()!.Contains(message)),
                It.Is<Exception>(e =>
                    exceptionPredicate == null || exceptionPredicate(e)
                ),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            )
        );
    }
}