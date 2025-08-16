using ExchangeRateUpdater.Domain.Ports;
using Microsoft.AspNetCore.Hosting.Server;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;
using WireMock.Server;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Unit;

internal class TestBase
{
    private TestHttpClientFactory? _httpClientFactory;
    protected Logger? _logger;
    private const int Port = 8080;
    protected WireMockServer? Server;

    protected IExchangeRateProviderRepository CreateSut()
    {
        return new CzechNationalBankRepositoryTestDouble(_httpClientFactory, _logger);
    }

    [SetUp]
    public void SetUp()
    {
        _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        _httpClientFactory = new TestHttpClientFactory($"http://localhost:{Port}/");
        Server = WireMockServer.Start(Port);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClientFactory?.Dispose();
        _logger?.Dispose();
        Server?.Stop();
        Server?.Dispose();
    }
}
