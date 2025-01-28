using ExchangeRateUpdater.Services;

[TestFixture]
public class ExchangeRatesServiceTests
{
    private ExchangeRatesService _exchangeRatesService;

    [SetUp]
    public void SetUp()
    {
        _exchangeRatesService = new ExchangeRatesService();
    }

    [Test]
    public void GetExchangeRatesAsync_ShouldNotReturnNull()
    {
        // Arrange
        DateTime date = new DateTime(2025, 01, 28);

        // Act
        var result = _exchangeRatesService.GetExchangeRatesAsync(date);

        // Assert
        Assert.NotNull(result);
    }

}