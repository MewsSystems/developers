using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using ApplicationLayer.Queries.Providers.GetAllProviders;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Providers;

public class GetAllProvidersQueryTests : IntegrationTestBase
{
    private async Task<int> CreateTestCurrencyAsync(string code)
    {
        var command = new CreateCurrencyCommand(code);
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();
        return result.Value;
    }

    private async Task<int> CreateTestProviderAsync(int currencyId, string code, string name, bool deactivate = false)
    {
        var command = new CreateExchangeRateProviderCommand(
            Name: name,
            Code: code,
            Url: $"https://api.{code.ToLower()}.com/rates",
            BaseCurrencyId: currencyId,
            RequiresAuthentication: false,
            ApiKeyVaultReference: null
        );
        var result = await Mediator.Send(command);
        result.IsSuccess.Should().BeTrue();

        if (deactivate)
        {
            await Mediator.Send(new DeactivateProviderCommand(result.Value));
        }

        return result.Value;
    }

    [Fact]
    public async Task GetAllProviders_WithNoProviders_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetAllProvidersQuery();

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetAllProviders_WithMultipleProviders_ShouldReturnAll()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");
        var eurId = await CreateTestCurrencyAsync("EUR");

        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Provider One");
        await CreateTestProviderAsync(eurId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Provider Two");
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Provider Three");

        // Act
        var query = new GetAllProvidersQuery(PageNumber: 1, PageSize: 10);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.Items.Should().Contain(p => p.Name == "Provider One");
        result.Items.Should().Contain(p => p.Name == "Provider Two");
        result.Items.Should().Contain(p => p.Name == "Provider Three");
    }

    [Fact]
    public async Task GetAllProviders_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");

        for (int i = 1; i <= 5; i++)
        {
            await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), $"Provider {i}");
        }

        // Act - Get page 2 with 2 items per page
        var query = new GetAllProvidersQuery(PageNumber: 2, PageSize: 2);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.TotalPages.Should().Be(3); // 5 items / 2 per page = 3 pages
    }

    [Fact]
    public async Task GetAllProviders_FilterByIsActive_ShouldReturnOnlyActive()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");

        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Active One", deactivate: false);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Inactive One", deactivate: true);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Active Two", deactivate: false);

        // Act
        var query = new GetAllProvidersQuery(IsActive: true);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(p => p.IsActive);
    }

    [Fact]
    public async Task GetAllProviders_FilterByIsInactive_ShouldReturnOnlyInactive()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");

        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Active One", deactivate: false);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Inactive One", deactivate: true);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Inactive Two", deactivate: true);

        // Act
        var query = new GetAllProvidersQuery(IsActive: false);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(p => !p.IsActive);
    }

    [Fact]
    public async Task GetAllProviders_FilterBySearchTerm_ShouldReturnMatchingProviders()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");

        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "European Central Bank");
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Czech National Bank");
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "National Bank of Romania");

        // Act - Search for "Central"
        var query = new GetAllProvidersQuery(SearchTerm: "Central");
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Contain("Central");
    }

    [Fact]
    public async Task GetAllProviders_FilterBySearchTermAndIsActive_ShouldCombineFilters()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");

        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Bank One", deactivate: false);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Bank Two", deactivate: true);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Bank Three", deactivate: false);
        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Other Provider", deactivate: false);

        // Act - Search for "Bank" and only active
        var query = new GetAllProvidersQuery(SearchTerm: "Bank", IsActive: true);
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.Should().OnlyContain(p => p.Name.Contains("Bank") && p.IsActive);
    }

    [Fact]
    public async Task GetAllProviders_ShouldIncludeBaseCurrencyCode()
    {
        // Arrange
        var usdId = await CreateTestCurrencyAsync("USD");
        var eurId = await CreateTestCurrencyAsync("EUR");

        await CreateTestProviderAsync(usdId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Provider One");
        await CreateTestProviderAsync(eurId, Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8), "Provider Two");

        // Act
        var query = new GetAllProvidersQuery();
        var result = await Mediator.Send(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(p => p.BaseCurrencyCode == "USD");
        result.Items.Should().Contain(p => p.BaseCurrencyCode == "EUR");
    }
}
