using ExchangeRateUpdater.Application.Cache;
using ExchangeRateUpdater.Application.GetExchangeRates;
using ExchangeRateUpdater.Domain;
using Moq;

namespace ExchangeRateUpdater.Application.Tests.GetExchangeRates;

public class GetExchangeRatesQueryHandlerTests
{
    private readonly Mock<ICzechNationalBankExchangeRateClient> _exchangeRateClientMock = new();
    private readonly Mock<ICzechNationalBankExchangeRateClientResponseConverter> _clientResponseConverterMock = new();
    private readonly Mock<IRedisClient> _redisClientMock = new();
    
    private GetExchangeRatesQueryHandler _sut => new(_exchangeRateClientMock.Object, _clientResponseConverterMock.Object, _redisClientMock.Object);

    [Fact]
    public async Task Handle_When_no_currencies_are_requested_Then_response_is_empty()
    {
        //Arrange
        var request = new GetExchangeRatesQuery();
        
        //Act
        var result = await _sut.Handle(request, default);
        
        //Assert
        Assert.Empty(result.ExchangeRates);
    }
    
    [Fact]
    public async Task Handle_When_invalid_currency_is_requested_Then_exception_is_thrown()
    {
        //Arrange
        var request = new GetExchangeRatesQuery { CurrencyCodes = ["INVALID"] };
        
        //Act
        var action = () => _sut.Handle(request, default);
        
        //Assert
        await Assert.ThrowsAsync<InvalidCurrencyCodeException>(action);
    }
    
    [Fact]
    public async Task Handle_When_request_has_no_date_Then_today_date_is_requested()
    {
        //Arrange
        var request = new GetExchangeRatesQuery{ CurrencyCodes = [ "EUR" ] };
        
        const string clientResponse = "ClientResponse";
        var requestDate = DateOnly.FromDateTime(DateTime.UtcNow).ToString();
        _redisClientMock
            .Setup(r => r.GetAsync(
                requestDate!,
                It.IsAny<Func<Task<string?>>>(),
                TimeSpan.FromHours(3)
            ))
            .ReturnsAsync(clientResponse);

        _clientResponseConverterMock.Setup(c => c.Convert(clientResponse)).Returns([]);
        
        //Act
        var result = await _sut.Handle(request, default);
        
        //Assert
        Assert.Empty(result.ExchangeRates);
    }
    
    [Fact]
    public async Task Handle_When_request_is_filled_Then_only_requested_exchange_rates_are_returned()
    {
        //Arrange
        const string euroCode = "EUR";
        var request = new GetExchangeRatesQuery
        {
            CurrencyCodes = [ euroCode ],
            Date = new DateOnly(2024, 2, 28)
        };
        
        const string clientResponse = "ClientResponse";
        var requestDate = request.Date.ToString();
        _redisClientMock
            .Setup(r => r.GetAsync(
                requestDate!,
                It.IsAny<Func<Task<string?>>>(),
                TimeSpan.FromHours(3)
            ))
            .ReturnsAsync(clientResponse);

        var euroCurrency = new Currency(euroCode);
        var dollarCurrency = new Currency("USD");
        var czechKorunaCurrency = new Currency("CZK");
        var allExchangeRates = new List<ExchangeRate>
        {
            new(euroCurrency, czechKorunaCurrency, 5),
            new(dollarCurrency, czechKorunaCurrency, 10)
        };
        _clientResponseConverterMock.Setup(c => c.Convert(clientResponse)).Returns(allExchangeRates);
        
        //Act
        var result = await _sut.Handle(request, default);
        
        //Assert
        Assert.Single(result.ExchangeRates);
        var expectedRate = allExchangeRates.Find(r => r.SourceCurrency.Code == euroCode)!;
        Assert.Equal(expectedRate.SourceCurrency.Code, result.ExchangeRates[0].SourceCurrency);
        Assert.Equal(expectedRate.TargetCurrency.Code, result.ExchangeRates[0].TargetCurrency);
        Assert.Equal(expectedRate.Value, result.ExchangeRates[0].Rate);
    }
}