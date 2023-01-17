using System.Collections.Generic;
using System.Linq;
using Application;
using Domain;

namespace UnitTests;

public class ExchangeRateProviderTests 
{
    private readonly List<CzechNationalBankExchangeRate> _getRatesResponse = new()
    {
        new()
        {
            Currency = "euro",
            Amount = 1,
            Code = "EUR",
            Country = "EMU",
            Rate = 24
        },

        new()
        {
            Currency = "dolar",
            Amount = 1,
            Code = "USD",
            Country = "USA",
            Rate = 22
        }
    };
    
    private Mock<ICzechNationalBankService> _czechNationalBankServiceMock;
    private ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTests()
    {
        _czechNationalBankServiceMock = new Mock<ICzechNationalBankService>();
        _czechNationalBankServiceMock
            .Setup(x => x.GetRates())
            .Returns(_getRatesResponse);
        
        _exchangeRateProvider = new ExchangeRateProvider(_czechNationalBankServiceMock.Object);
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