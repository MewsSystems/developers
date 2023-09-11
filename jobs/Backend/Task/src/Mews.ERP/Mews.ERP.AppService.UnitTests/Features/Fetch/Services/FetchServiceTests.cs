using AutoFixture;
using FluentAssertions;
using Mews.ERP.AppService.Data.Models;
using Mews.ERP.AppService.Features.Fetch.Models;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Repositories.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Services;
using Mews.ERP.AppService.Features.Fetch.Services.Interfaces;
using Moq;
using Moq.AutoMock;

namespace Mews.ERP.AppService.UnitTests.Features.Fetch.Services;

public class FetchServiceTests
{
    private readonly AutoMocker autoMocker = new();
    
    private readonly Fixture fixture = new();

    private readonly IFetchService sut;

    public FetchServiceTests()
    {
        sut = autoMocker.CreateInstance<FetchService>();
    }

    [Fact]
    public async Task When_GetExchangeRatesAsync_Is_Called_Service_Should_Get_ExchangeRates_For_Currencies()
    {
        // Arrange
        var mockedCurrencies = fixture
            .CreateMany<Currency>(20)
            .AsQueryable();
        
        var mockedRates = fixture.CreateMany<ExchangeRate>(20);

        autoMocker
            .GetMock<ICurrenciesRepository>()
            .Setup(cr => cr.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockedCurrencies);

        autoMocker
            .GetMock<ICnbExchangeRatesProvider>()
            .Setup(erp => erp.GetExchangeRatesAsync(mockedCurrencies, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockedRates);

        // Act
        var result = await sut.GetExchangeRatesAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(mockedRates.Count());
        
        autoMocker
            .Verify<ICurrenciesRepository>(
                cr => cr.GetAllAsync(It.IsAny<CancellationToken>()), 
                Times.Once
            );
        
        autoMocker
            .Verify<ICnbExchangeRatesProvider>(
                erp => erp.GetExchangeRatesAsync(mockedCurrencies, It.IsAny<CancellationToken>()),
                Times.Once
            );
    }
}