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
internal class GetDefaultUnitRatesTests : TestBase
{

    private const string TestFilesPath = "TestFiles/GetDefaultUnitRates";

    [Test]
    public void GivenNoHeaderData_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve header data from Czech National Bank.";
        Server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoHeaderData.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetAllFxRates(new DateTime(), CancellationToken.None));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public void GivenNoAmountHeader_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve Amount from document.";

        Server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoAmountHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetAllFxRates(new DateTime(), CancellationToken.None));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public void GivenNoCodeHeader_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve Code from document.";

        Server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoCodeHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetAllFxRates(new DateTime(), CancellationToken.None));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public void GivenNoRateHeader_ShouldThrowFormatException()
    {
        // arrange
        var expected = "Couldn't retrieve Rate from document.";

        Server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenNoRateHeader.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.GetAllFxRates(new DateTime(), CancellationToken.None));
        exception!.Message.Should().Be(expected);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be(expected);
    }

    [Test]
    public async Task GivenInconsistentColumns_ShouldThrowFormatException()
    {
        // arrange
        Server!.Given(
            Request.Create().UsingGet().WithPath("/Test"))
            .RespondWith(Response.Create().WithBodyFromFile($"{TestFilesPath}/GivenInconsistentColumns.txt").WithHeader("Content-Type", "text/plain").WithStatusCode(200));

        // act
        var sut = CreateSut();
        var result = await sut.GetAllFxRates(new DateTime(), CancellationToken.None);
        
        // assert
        result.Should().BeEmpty();
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Could not parse line {LineNumber}. The line start with: {LineText}");
    }
}
