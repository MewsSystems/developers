using ExchangeRateUpdater.Domain.Ports;
using FluentAssertions;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Adapter.ExchangeRateProvider.CzechNationalBank.Tests.Unit;

[TestFixture]
internal class GetDefaultUnitRatesTests
{
    private TestHttpClientFactory? _httpClientFactory;
    private Logger? _logger;
    private const int Port = 8080; 
    private WireMockServer? _server;
    private const string RelativePath = "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt&date=01.01.01";

    [Test]
    public void GivenNoHeaderData_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve header data from Czech National Bank.";
        _server!.Given(
            Request.Create().UsingGet().WithPath(RelativePath))
            .RespondWith(Response.Create().WithBody(File.ReadAllText("TestFiles/GivenNoHeaderData_ShouldThrowFormatException.txt")).WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetDefaultUnitRates());
        exception!.Message.Should().Be(expected);
    }

    private IExchangeRateProviderRepository CreateSut()
    {
        return new CzechNationalBankRepositoryTestDouble(_httpClientFactory, _logger);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        _httpClientFactory = new TestHttpClientFactory("http://localhost:8080/");
        _server = WireMockServer.Start(Port);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _httpClientFactory?.Dispose();
        _logger?.Dispose();
        _server?.Stop();
        _server?.Dispose();
    }
}
