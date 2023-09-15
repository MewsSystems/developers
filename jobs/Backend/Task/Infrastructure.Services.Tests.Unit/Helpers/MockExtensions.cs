using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Text.RegularExpressions;

namespace Infrastructure.Services.Tests.Unit.Helpers;

internal static class MockExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel level, Times times, string? regex = null) =>
        logger.Verify(m => m.Log(
        level,
        It.IsAny<EventId>(),
        It.Is<It.IsAnyType>((x, y) => regex == null || Regex.IsMatch(x.ToString() ?? "", regex)),
        It.IsAny<Exception?>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        times);
}
