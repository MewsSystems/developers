using ExchangeRateProvider.Application.Services;

namespace ExchangeRateProvider.Host.WebApiIntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using Application.Interfaces;
using Domain.Options;
using WebApi;
using WebApi.Models;
using Infrastructure.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[TestFixture]
public class ExchangeRateControllerTests
{
    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IExchangeRateProvider, CnbExchangeRateProvider>();
                });
            });

        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [Test]
    public void VerifyContainer_ResolvesDependenciesSuccessfully()
    {
        // Arrange & Act
        var action = _factory.Invoking(x => x.CreateClient());

        var resolveServicesActions = new List<Action>
        {
            () => _factory.Services.GetRequiredService<IExchangeRateProvider>(),
            () => _factory.Services.GetRequiredService<IOptions<CnbApiOptions>>(),
            () => _factory.Services.GetRequiredService<IMemoryCache>(),
            () => _factory.Services.GetRequiredService<ILogger<ExchangeRateBackgroundService>>()
        };

        // Assert
        using (new AssertionScope())
        {
            action.Should().NotThrow();
            resolveServicesActions.ForEach(a =>
                a.Should().NotThrow("All required dependencies should be resolved properly"));
        }
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnOk()
    {
        // Arrange
        var request = new { Items = new[] { "USD", "EUR", "CZK" } };

        // Act
        var response = await _client.PostAsJsonAsync("/api/exchangerates", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetExchangeRatesResponse>();
        result.Should().NotBeNull();
        result.Rates.Should().NotBeEmpty();
        result.CurrenciesNotResolved.Should().BeEmpty();
    }

    [Test]
    public async Task GetExchangeRates_ShouldReturnBadRequest_WhenEmptyRequest()
    {
        // Arrange
        var request = new { Items = new string[] { } };

        // Act
        var response = await _client.PostAsJsonAsync("/api/exchangerates", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
