using AutoFixture;
using FluentAssertions;
using Mews.ERP.Api.Controllers;
using Mews.ERP.AppService.Features.Fetch.Models;
using Mews.ERP.AppService.Features.Fetch.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace Mews.ERP.Api.UnitTests.Controllers;

public class FetchControllerTests
{
    private readonly AutoMocker autoMocker = new();

    private readonly Fixture fixture = new();
    
    private readonly FetchController sut;

    public FetchControllerTests()
    {
        sut = autoMocker.CreateInstance<FetchController>();
    }

    [Fact]
    public async Task When_Fetch_Endpoint_Is_Called_Ok_Result_With_ExchangeRates_Should_Be_Returned()
    {
        // Arrange
        var exchangeRates = fixture.CreateMany<ExchangeRate>(15);
        var fetchServiceMock = autoMocker.GetMock<IFetchService>();
        
        fetchServiceMock
            .Setup(fs => fs.GetExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);
        
        // Act
        var result = await sut.Fetch();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        ((IEnumerable<ExchangeRate>) ((OkObjectResult) result).Value!).Count().Should().Be(exchangeRates.Count());
        
        fetchServiceMock
            .Verify(
                fs => fs.GetExchangeRatesAsync(),
                Times.Once
            );
    }
}