using Application;
using Domain;

namespace UnitTests;

public class ExchangeRateProviderTests 
{
    private readonly List<CzechNationalBankExchangeRate> _getRatesResponse = new()
    {
        new CzechNationalBankExchangeRate
        {
            Currency = "euro",
            Amount = 1,
            Code = "EUR",
            Country = "EMU",
            Rate = 24
        },

        new CzechNationalBankExchangeRate
        {
            Currency = "dolar",
            Amount = 1,
            Code = "USD",
            Country = "USA",
            Rate = 22
        }
    };
    
    private readonly ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTests()
    {
        var czechNationalBankServiceMock = new Mock<ICzechNationalBankService>();
        czechNationalBankServiceMock
            .Setup(x => x.GetRates())
            .Returns(_getRatesResponse);
        
        _exchangeRateProvider = new ExchangeRateProvider(czechNationalBankServiceMock.Object);
    }
    
    [Fact]
    public void ShouldReturnExchangeRates()
    {
        var result = _exchangeRateProvider
            .GetExchangeRates(new[] { new Currency("USD"), new Currency("EUR") })
            .ToList();
        
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public void ShouldReturnEmptyListWhenNoData()
    {
        var result = _exchangeRateProvider
            .GetExchangeRates(new List<Currency>());
        
        result.Should().BeEmpty();
    }
}