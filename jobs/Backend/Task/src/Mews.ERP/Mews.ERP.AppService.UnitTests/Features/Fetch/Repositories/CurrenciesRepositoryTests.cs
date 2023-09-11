using FluentAssertions;
using Mews.ERP.AppService.Features.Fetch.Repositories;
using Mews.ERP.AppService.Features.Fetch.Repositories.Interfaces;

namespace Mews.ERP.AppService.UnitTests.Features.Fetch.Repositories;

public class CurrenciesRepositoryTests
{
    private readonly ICurrenciesRepository sut = new CurrenciesRepository();

    [Fact]
    public async Task When_GetAllAsync_Method_Is_Called_A_List_Of_Currencies_Should_Be_Returned()
    {
        // Act
        var results = await sut.GetAllAsync();

        // Assert
        results.Should().NotBeNull();
        results.Any(x => x.Code.Equals("USD")).Should().BeTrue();
    }
}