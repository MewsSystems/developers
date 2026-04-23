using ExchangeRateUpdater.Infrastructure.Common;
using ExchangeRateUpdater.Infrastructure.Common.Configuration;
using ExchangeRateUpdater.Infrastructure.CzechNationalBankExchangeRates;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Infrastructure.Tests.CzechNationalBankExchangeRates;

public class CzechNationalBankExchangeRateClientTests
{
    private readonly Mock<IRestClient> _restClientMock = new();
    private readonly Mock<IOptions<InfrastructureOptions>> _infrastructureOptionsMock = new();

    private CzechNationalBankExchangeRateClient _sut => new(_restClientMock.Object, _infrastructureOptionsMock.Object);

    [Fact]
    public async Task GetAsync_When_requested_Then_result_is_as_expected()
    {
        //Arrange
        var now = DateTime.UtcNow;
        var date = DateOnly.FromDateTime(now);
        
        const string url = "url";
        var options = new InfrastructureOptions { CzechNationalBankExchangeRateService = new() { Url = url } };
        _infrastructureOptionsMock.Setup(i => i.Value).Returns(options);
        const string? expectedResult = "restClientResult";
        _restClientMock.Setup(r => r.GetAsync<string?>(It.Is<string>(s => s == $"{url}?date={now:dd.MM.yyyy}"))).ReturnsAsync(expectedResult);

        //Act
        var result = await _sut.GetAsync(date);
        
        //Assert
        Assert.Equal(expectedResult, result);
    }
}