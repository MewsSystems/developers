using ExchangeRateProvider.Domain.ValueObjects;
using ExchangeRateProvider.Infrastructure.ExternalServices.CZK;
using FakeItEasy;
using Microsoft.Extensions.Logging.Testing;
using Shouldly;

namespace ExchangeRateProvider.Infrastructure.Tests.Unit.ExternalServices.CZK;

public class CzkExchangeRateServiceTests
{
    private readonly ICzkApiClient _apiClient;
    private readonly ICzkExchangeRateMapper _mapper;

    public CzkExchangeRateServiceTests()
    {
        _apiClient = A.Fake<ICzkApiClient>();
        _mapper = A.Fake<ICzkExchangeRateMapper>();
    }

    [Fact]
    public async Task GetExchangeRatesAsync_WithValidResponse_ReturnsMappedRates()
    {
        // Arrange
        var response = new CzkExchangeRateResponse(new List<CzkRate>());
        var expectedRates = new List<ExchangeRate>
        {
            new(new Currency("CZK"), new Currency("USD"), 0.042m)
        };
        
        A.CallTo(() => _apiClient.GetExchangeRatesAsync(A<CancellationToken>._)).Returns(response);
        A.CallTo(() => _mapper.MapToExchangeRates(response, A<Currency>._)).Returns(expectedRates);
        
        var service = new CzkExchangeRateService(_apiClient, _mapper);

        // Act
        var result = await service.GetExchangeRatesAsync(new Currency("CZK"), CancellationToken.None);

        // Assert
        result.ShouldBe(expectedRates);
        A.CallTo(() => _apiClient.GetExchangeRatesAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _mapper.MapToExchangeRates(response, A<Currency>._)).MustHaveHappenedOnceExactly();
    }
}
