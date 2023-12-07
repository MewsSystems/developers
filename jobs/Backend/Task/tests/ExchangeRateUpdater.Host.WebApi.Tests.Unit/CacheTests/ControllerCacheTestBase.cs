using Adapter.ExchangeRateProvider.InMemory;
using ExchangeRateUpdater.Host.WebApi.Tests.Unit.CacheTests;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Serilog;
using Serilog.Sinks.InMemory;


namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit;

internal abstract class ControllerCacheTestBase
{
    protected IHost? Host;
    protected TestServer? Server;
    protected HttpClient? HttpClient;
    protected const string ApiBaseAddress = "http://exchange-rate-update.com";
    protected ExchangeRateProviderRepositoryInMemory? ExchangeRateProviderRepository;
    protected ExchangeRateCacheRepositoryInMemory? ExchangeRateCacheRepository;
    protected Serilog.Core.Logger? Logger;
    protected TimeSpan TodayTtl = TimeSpan.FromSeconds(10);
    protected TimeSpan OtherDatesTtl = TimeSpan.FromDays(10);
    protected ReferenceTimeTestDouble ReferenceTime = new ReferenceTimeTestDouble();

    [SetUp]
    public async Task SetUp()
    {
        Logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        ExchangeRateProviderRepository = new ExchangeRateProviderRepositoryInMemory();
        var settings = new Configuration.Settings()
        {
            CacheEnabled = true,
            CacheSize = 2,
            TodayDataCacheTtl = TodayTtl,
            OtherDatesCacheTtl = OtherDatesTtl,
        };
        ExchangeRateCacheRepository = new ExchangeRateCacheRepositoryInMemory(ExchangeRateProviderRepository, Logger, settings.CacheSize, 
                                                            settings.CacheEnabled, settings.TodayDataCacheTtl, settings.OtherDatesCacheTtl, ReferenceTime);
        var hostBuilder = new TestApplicationHostBuilder(ExchangeRateProviderRepository,
                                                         settings, Logger, ReferenceTime, ExchangeRateCacheRepository);
        Host = hostBuilder.Configure().Build();
        await Host.StartAsync();
        Server = Host.GetTestServer();
        Server.BaseAddress = new Uri(ApiBaseAddress);
        HttpClient = Server.CreateClient();
    }

    [TearDown]
    public async Task TearDown()
    {
        Logger?.Dispose();
        HttpClient?.Dispose();
        Server?.Dispose();
        await Host!.StopAsync();
        Host?.Dispose();
    }
}
