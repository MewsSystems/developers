using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Unit;

[TestFixture]
internal class GetExchangeRateForCurrenciesAsyncTests
{
    private TestHttpClientFactory? _httpClientFactory;
    private Logger? _logger;
    private const int Port = 8080;
    private WireMockServer? _server;
    private const string TestFilesPath = "TestFiles/GetExchangeRateForCurrenciesAsync";

    [Test]
    public void GivenNoCurrencyInfoLine_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoCurrencyLine.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't not retrieve currency info line.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't not retrieve currency info line.");
    }

    [Test]
    public void GivenInvalidCurrencyInfoLine_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenInvalidCurrencyLine.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't not retrieve currency info line.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't not retrieve currency info line.");
    }

    [Test]
    public void GivenInvalidCurrencyFirstPart_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/BadCurrencyFirstPart.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't not retrieve currency code and/or amount.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't not retrieve currency code and/or amount. Line: {Line}");
    }

    [Test]
    public void GivenInvalidCurrencySecondPart_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/BadCurrencySecondPart.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't not retrieve currency code and/or amount.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't not retrieve currency code and/or amount. Line: {Line}");
    }

    [Test]
    public void GivenInvalidCurrencyCode_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenInvalidCurrencyCode.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("The requested currency is different than the one parsed. Requested: USD. Retrieved: ");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("The requested currency is different than the one parsed. Requested: {RequestedCurrency}. Retrieved: {RetrievedCurrency}");
    }

    [Test]
    public void GivenInvalidAmount_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenInvalidAmount.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't not convert amount in a number.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't not convert amount in a number. Line: {Line}");
    }

    [Test]
    public void GivenNoDateHeader_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoDateHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't retrieve Date from document.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't retrieve Date from document.");
    }

    [Test]
    public void GivenNoRateHeader_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoRateHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't retrieve Rate from document.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't retrieve Rate from document.");
    }

    [Test]
    public void GivenNoHeader_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime()));

        // assert
        exception!.Message.Should().Be("Couldn't retrieve header data from Czech National Bank.");
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Couldn't retrieve header data from Czech National Bank.");
    }

    [Test]
    public async Task GivenInconsistentColumns_ShouldThrowFormatException()
    {
        // arrange
        _server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenInconsistentColumns.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var result = await sut.GetExchangeRateForCurrenciesAsync(new Currency("USD"), new Currency("CZK"), new DateTime(), new DateTime());

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
