using ExchangeRateUpdater.Exceptions;
using Shouldly;

namespace ExchangeRateUpdater.Tests.Exceptions;

public class ExternalServiceReturnedNoExchangeRatesExceptionTests
{
    [Fact]
    public void ShouldSetUrlProperty()
    {
        // Arrange
        string url = "https://api.example.com/exchange-rates";

        // Act
        NoExchangeRatesReceivedException exception = new(url);

        // Assert
        exception.Url.ShouldBe(url);
    }
    [Fact]
    public void ShouldHaveCorrectMessage()
    {
        // Arrange
        string url = "https://api.example.com/exchange-rates";

        // Act
        NoExchangeRatesReceivedException exception = new(url);

        // Assert
        exception.Message.ShouldBe("External service returned null or an empty list of exchange rates.");
    }
}
