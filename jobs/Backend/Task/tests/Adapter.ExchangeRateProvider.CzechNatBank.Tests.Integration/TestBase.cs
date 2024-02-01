using ExchangeRateUpdater.Domain.Ports;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;

internal class TestBase
{
    private TestHttpClientFactory? _httpClientFactory;
    private Logger? _logger;

    protected IExchangeRateProviderRepository CreateSut()
    {
        return new CzechNationalBankRepository(_httpClientFactory, _logger);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        _httpClientFactory = new TestHttpClientFactory(Global.Settings!.CzechNationalBankBaseAddress);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _httpClientFactory?.Dispose();
        _logger?.Dispose();
    }
}
