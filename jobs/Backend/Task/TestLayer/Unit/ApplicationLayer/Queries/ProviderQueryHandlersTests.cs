using ApplicationLayer.DTOs.ExchangeRateProviders;
using ApplicationLayer.Queries.Providers.GetAllProviders;
using ApplicationLayer.Queries.Providers.GetProviderById;
using ApplicationLayer.Queries.Providers.GetProviderConfiguration;
using ApplicationLayer.Queries.Providers.GetProviderHealth;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.Enums;
using DomainLayer.Models.Views;
using DomainLayer.ValueObjects;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for Provider query handlers.
/// Tests GetAllProviders, GetProviderById, GetProviderConfiguration, GetProviderHealth.
/// </summary>
public class ProviderQueryHandlersTests : TestBase
{
    #region GetAllProvidersQueryHandler Tests

    [Fact]
    public async Task GetAllProviders_WithProviders_ShouldReturnPagedResults()
    {
        // Arrange
        var handler = new GetAllProvidersQueryHandler(MockUnitOfWork.Object);
        var query = new GetAllProvidersQuery(PageNumber: 1, PageSize: 10);

        var providers = new List<ExchangeRateProvider>
        {
            ExchangeRateProvider.Create("ECB", "ECB", "https://ecb.europa.eu", 1),
            ExchangeRateProvider.Create("CNB", "CNB", "https://cnb.cz", 2),
            ExchangeRateProvider.Create("NBP", "NBP", "https://nbp.pl", 3)
        };

        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(providers[0], 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(providers[1], 2);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(providers[2], 3);

        var currencies = new List<Currency>
        {
            Currency.FromCode("EUR", id: 1),
            Currency.FromCode("CZK", id: 2),
            Currency.FromCode("PLN", id: 3)
        };

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)providers);

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)currencies);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);

        var firstProvider = result.Items.First();
        firstProvider.Code.Should().Be("ECB");
        firstProvider.BaseCurrencyCode.Should().Be("EUR");
    }

    [Fact]
    public async Task GetAllProviders_WithIsActiveFilter_ShouldFilterResults()
    {
        // Arrange
        var handler = new GetAllProvidersQueryHandler(MockUnitOfWork.Object);
        var query = new GetAllProvidersQuery(IsActive: true);

        var provider1 = ExchangeRateProvider.Create("Active", "ACT", "https://active.com", 1);
        var provider2 = ExchangeRateProvider.Create("Inactive", "INA", "https://inactive.com", 1);

        provider2.Deactivate(); // Make inactive

        var providers = new List<ExchangeRateProvider> { provider1, provider2 };

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)providers);

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)new List<Currency> { Currency.FromCode("EUR", id: 1) });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Code.Should().Be("ACT");
        result.Items.First().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllProviders_WithSearchTerm_ShouldFilterByNameCodeOrUrl()
    {
        // Arrange
        var handler = new GetAllProvidersQueryHandler(MockUnitOfWork.Object);
        var query = new GetAllProvidersQuery(SearchTerm: "ECB");

        var providers = new List<ExchangeRateProvider>
        {
            ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1),
            ExchangeRateProvider.Create("Czech National Bank", "CNB", "https://cnb.cz", 1),
            ExchangeRateProvider.Create("Bank", "BANK", "https://ecb.com", 1) // URL contains "ecb"
        };

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)providers);

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)new List<Currency> { Currency.FromCode("EUR", id: 1) });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2); // ECB code and ecb.com URL
        result.Items.Should().Contain(p => p.Code == "ECB");
        result.Items.Should().Contain(p => p.Code == "BANK");
    }

    [Fact]
    public async Task GetAllProviders_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var handler = new GetAllProvidersQueryHandler(MockUnitOfWork.Object);
        var query = new GetAllProvidersQuery(PageNumber: 2, PageSize: 2);

        var providers = new List<ExchangeRateProvider>
        {
            ExchangeRateProvider.Create("Provider1", "P1", "https://p1.com", 1),
            ExchangeRateProvider.Create("Provider2", "P2", "https://p2.com", 1),
            ExchangeRateProvider.Create("Provider3", "P3", "https://p3.com", 1),
            ExchangeRateProvider.Create("Provider4", "P4", "https://p4.com", 1),
            ExchangeRateProvider.Create("Provider5", "P5", "https://p5.com", 1)
        };

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)providers);

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)new List<Currency> { Currency.FromCode("EUR", id: 1) });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2); // Page 2, size 2: items 3-4
        result.TotalCount.Should().Be(5);
        result.Items.Select(p => p.Code).Should().ContainInOrder("P3", "P4");
    }

    [Fact]
    public async Task GetAllProviders_WithNoProviders_ShouldReturnEmptyResult()
    {
        // Arrange
        var handler = new GetAllProvidersQueryHandler(MockUnitOfWork.Object);
        var query = new GetAllProvidersQuery();

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ExchangeRateProvider>)new List<ExchangeRateProvider>());

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)new List<Currency>());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion

    #region GetProviderByIdQueryHandler Tests

    [Fact]
    public async Task GetProviderById_WithExistingProvider_ShouldReturnDetailDto()
    {
        // Arrange
        var handler = new GetProviderByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetProviderByIdQueryHandler>().Object);

        var query = new GetProviderByIdQuery(ProviderId: 1);

        var provider = ExchangeRateProvider.Create("ECB", "ECB", "https://ecb.europa.eu", 1, true, "vault-key");
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        provider.SetConfiguration("api_version", "v1", "API version");

        var currency = Currency.FromCode("EUR", id: 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Code.Should().Be("ECB");
        result.BaseCurrencyCode.Should().Be("EUR");
        result.RequiresAuthentication.Should().BeTrue();
        result.ApiKeyVaultReference.Should().Be("vault-key");
        result.Configurations.Should().HaveCount(1);
        result.Configurations.First().SettingKey.Should().Be("api_version");
    }

    [Fact]
    public async Task GetProviderById_WithNonExistentProvider_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetProviderByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetProviderByIdQueryHandler>().Object);

        var query = new GetProviderByIdQuery(ProviderId: 999);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderById_WithMissingCurrency_ShouldReturnUnknownCurrency()
    {
        // Arrange
        var handler = new GetProviderByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetProviderByIdQueryHandler>().Object);

        var query = new GetProviderByIdQuery(ProviderId: 1);

        var provider = ExchangeRateProvider.Create("Test", "TEST", "https://test.com", 999);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.BaseCurrencyCode.Should().Be("UNKNOWN");
    }

    #endregion

    #region GetProviderConfigurationQueryHandler Tests

    [Fact]
    public async Task GetProviderConfiguration_WithExistingProvider_ShouldReturnConfiguration()
    {
        // Arrange
        var handler = new GetProviderConfigurationQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetProviderConfigurationQueryHandler>().Object);

        var query = new GetProviderConfigurationQuery(ProviderId: 1);

        var provider = ExchangeRateProvider.Create("ECB", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        provider.SetConfiguration("timeout", "30", "Request timeout in seconds");
        provider.SetConfiguration("retries", "3", "Number of retry attempts");

        var currency = Currency.FromCode("EUR", id: 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Configurations.Should().HaveCount(2);
        result.Configurations.Should().Contain(c => c.SettingKey == "timeout" && c.SettingValue == "30");
        result.Configurations.Should().Contain(c => c.SettingKey == "retries" && c.SettingValue == "3");
    }

    [Fact]
    public async Task GetProviderConfiguration_WithNonExistentProvider_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetProviderConfigurationQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetProviderConfigurationQueryHandler>().Object);

        var query = new GetProviderConfigurationQuery(ProviderId: 999);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderConfiguration_WithNoConfigurations_ShouldReturnEmptyList()
    {
        // Arrange
        var handler = new GetProviderConfigurationQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetProviderConfigurationQueryHandler>().Object);

        var query = new GetProviderConfigurationQuery(ProviderId: 1);

        var provider = ExchangeRateProvider.Create("ECB", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, 1);

        var currency = Currency.FromCode("EUR", id: 1);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Configurations.Should().BeEmpty();
    }

    #endregion

    #region GetProviderHealthQueryHandler Tests

    [Fact]
    public async Task GetProviderHealth_WithHealthyProvider_ShouldReturnHealthDto()
    {
        // Arrange
        var handler = new GetProviderHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetProviderHealthQueryHandler>().Object);

        var query = new GetProviderHealthQuery(ProviderId: 1);

        var providerHealth = new ProviderHealthStatusView
        {
            Id = 1,
            Code = "ECB",
            Name = "European Central Bank",
            HealthStatus = "Healthy",
            ConsecutiveFailures = 0,
            LastSuccessfulFetch = new DateTimeOffset(2025, 11, 6, 10, 0, 0, TimeSpan.Zero),
            LastFailedFetch = null
        };

        var now = new DateTimeOffset(2025, 11, 6, 12, 0, 0, TimeSpan.Zero);
        MockDateTimeProvider.Setup(x => x.UtcNow).Returns(now);

        MockViewQueries
            .Setup(x => x.GetProviderHealthStatusByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(providerHealth);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.ProviderId.Should().Be(1);
        result.ProviderCode.Should().Be("ECB");
        result.Status.Should().Be("Healthy");
        result.IsHealthy.Should().BeTrue();
        result.ConsecutiveFailures.Should().Be(0);
        result.TimeSinceLastSuccess.Should().Be(TimeSpan.FromHours(2));
        result.TimeSinceLastFailure.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderHealth_WithUnhealthyProvider_ShouldReturnUnhealthyStatus()
    {
        // Arrange
        var handler = new GetProviderHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetProviderHealthQueryHandler>().Object);

        var query = new GetProviderHealthQuery(ProviderId: 1);

        var providerHealth = new ProviderHealthStatusView
        {
            Id = 1,
            Code = "FAIL",
            Name = "Failing Provider",
            HealthStatus = "Unhealthy",
            ConsecutiveFailures = 3,
            LastSuccessfulFetch = new DateTimeOffset(2025, 11, 5, 10, 0, 0, TimeSpan.Zero),
            LastFailedFetch = new DateTimeOffset(2025, 11, 6, 11, 30, 0, TimeSpan.Zero)
        };

        var now = new DateTimeOffset(2025, 11, 6, 12, 0, 0, TimeSpan.Zero);
        MockDateTimeProvider.Setup(x => x.UtcNow).Returns(now);

        MockViewQueries
            .Setup(x => x.GetProviderHealthStatusByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(providerHealth);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be("Unhealthy");
        result.IsHealthy.Should().BeFalse();
        result.ConsecutiveFailures.Should().Be(3);
        result.TimeSinceLastSuccess.Should().Be(TimeSpan.FromHours(26));
        result.TimeSinceLastFailure.Should().Be(TimeSpan.FromMinutes(30));
    }

    [Fact]
    public async Task GetProviderHealth_WithNonExistentProvider_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetProviderHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetProviderHealthQueryHandler>().Object);

        var query = new GetProviderHealthQuery(ProviderId: 999);

        MockViewQueries
            .Setup(x => x.GetProviderHealthStatusByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProviderHealthStatusView?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProviderHealth_WithNoFetchHistory_ShouldHandleNullTimestamps()
    {
        // Arrange
        var handler = new GetProviderHealthQueryHandler(
            MockViewQueries.Object,
            MockDateTimeProvider.Object,
            CreateMockLogger<GetProviderHealthQueryHandler>().Object);

        var query = new GetProviderHealthQuery(ProviderId: 1);

        var providerHealth = new ProviderHealthStatusView
        {
            Id = 1,
            Code = "NEW",
            Name = "New Provider",
            HealthStatus = "Pending",
            ConsecutiveFailures = 0,
            LastSuccessfulFetch = null,
            LastFailedFetch = null
        };

        MockViewQueries
            .Setup(x => x.GetProviderHealthStatusByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(providerHealth);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be("Pending");
        result.TimeSinceLastSuccess.Should().BeNull();
        result.TimeSinceLastFailure.Should().BeNull();
    }

    #endregion
}
