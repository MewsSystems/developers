using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Extensions;

public static class LoggerExtensions
{
    public static void VerifyLogged<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel level,
        string messageSubstring,
        Times? times = null)
    {
        times ??= Times.Once();

        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(messageSubstring)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times.Value);
    }
}