using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Tests.Shared.Builders;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Domain.Tests.Providers;

public class ExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateProviderClient> _client;
    private readonly ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTests()
    {
        _client = new Mock<IExchangeRateProviderClient>();
        _exchangeRateProvider = new ExchangeRateProvider(_client.Object);
    }

    [Theory]
    [MemberData(nameof(MappingTestCases))]
    public async Task Given_currencies_when_retrieving_exchange_rates_then_return_ok_with_appropriate_content(
        List<ExchangeRate> exchangeRates, List<Currency> currencies, List<ExchangeRate> expectedResult)
    {
        //Arrange
        _client.Setup(client => client.GetExchangeRatesAsync()).ReturnsAsync(exchangeRates);

        //Act
        var result = await _exchangeRateProvider.GetExchangeRates(currencies);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    public static IEnumerable<object[]> MappingTestCases()
    {
        yield return new object[]
        {
            new List<ExchangeRate>
            {
                new ExchangeRateBuilder().WithValue(1).WithSourceCurrency("USD").WithTargetCurrency("CZK").Build(),
                new ExchangeRateBuilder().WithValue(1).WithSourceCurrency("TRY").WithTargetCurrency("CZK").Build()
            },
            new List<Currency>
            {
                new("USD")
            },
            new List<ExchangeRate>
            {
                new ExchangeRateBuilder().WithValue(1).WithSourceCurrency("USD").WithTargetCurrency("CZK").Build()
            }
        };
        
        yield return new object[]
        {
            new List<ExchangeRate>
            {
                new ExchangeRateBuilder().WithValue(1).WithSourceCurrency("USD").WithTargetCurrency("CZK").Build(),
            },
            new List<Currency>
            {
                new("TRY")
            },
            new List<ExchangeRate>()
        };
    }
}