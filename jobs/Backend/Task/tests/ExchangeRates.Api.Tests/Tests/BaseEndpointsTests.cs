using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;

namespace ExchangeRates.Api.Tests.Tests;

public class BaseEndpointsTests : IAsyncLifetime
{
    private readonly ExchangeRatesTestsContext _exchangeRatesTestsContext;

    public BaseEndpointsTests(ExchangeRatesTestsContext exchangeRatesTestsContext)
    {
        _exchangeRatesTestsContext = exchangeRatesTestsContext;
    }

    public async Task DisposeAsync()
    {
        ResetHttpClientMock();
        await ResetCacheAsync();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    private async Task ResetCacheAsync()
    {
        var cache = _exchangeRatesTestsContext.Services.GetRequiredService<IDistributedCache>();
        await cache.RemoveAsync("exchange-rates");
    }

    private void ResetHttpClientMock()
    {
        var wireMockServer = _exchangeRatesTestsContext.Services.GetRequiredService<WireMockServer>();
        wireMockServer.Reset();
    }
}