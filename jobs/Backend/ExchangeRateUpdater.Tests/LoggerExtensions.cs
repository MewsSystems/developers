using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests;
public static class LoggerExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string messageFragment, Times times)
    {
        loggerMock.Verify(x => x.Log(
            level,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(messageFragment)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            times);
    }
}