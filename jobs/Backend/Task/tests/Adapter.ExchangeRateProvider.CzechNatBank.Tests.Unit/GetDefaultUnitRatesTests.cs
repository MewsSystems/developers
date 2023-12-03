using ExchangeRateUpdater.Domain.Ports;
using FluentAssertions;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Unit;

[TestFixture]
internal class GetDefaultUnitRatesTests
{
    private TestHttpClientFactory? _httpClientFactory;
    private Logger? _logger;
    private const int Port = 8080;
    private WireMockServer? _server;

    [Test]
    public void GivenNoHeaderData_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve header data from Czech National Bank.";
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile("TestFiles/GivenNoHeaderData.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetDefaultUnitRates(new DateTime()));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public void GivenNoAmountHeader_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve Amount from document.";

        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile("TestFiles/GivenNoAmountHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetDefaultUnitRates(new DateTime()));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public void GivenNoCodeHeader_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve Code from document.";

        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile("TestFiles/GivenNoCodeHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetDefaultUnitRates(new DateTime()));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public void GivenNoRateHeader_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve Rate from document.";

        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile("TestFiles/GivenNoRateHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetDefaultUnitRates(new DateTime()));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public async Task GivenInconsistentColumns_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile("TestFiles/GivenInconsistentColumns.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act
        var sut = CreateSut();
        var result = await sut.GetDefaultUnitRates(new DateTime());
        
        // assert
        result.Should().BeEmpty();
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Could not parse line {LineNumber}. The line start with: {LineText}");
    }

    private IExchangeRateProviderRepository CreateSut()
    {
        return new CzechNationalBankRepositoryTestDouble(_httpClientFactory, _logger);
    }

    [SetUp]
    public void OneTimeSetUp()
    {
        _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        _httpClientFactory = new TestHttpClientFactory("http://localhost:8080/");
        _server = WireMockServer.Start(Port);
    }

    [TearDown]
    public void OneTimeTearDown()
    {
        _httpClientFactory?.Dispose();
        _logger?.Dispose();
        _server?.Stop();
        _server?.Dispose();
    }
}
