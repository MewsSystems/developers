using FluentAssertions;
using NUnit.Framework;
using Serilog;
using Serilog.Sinks.InMemory;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Unit;

[TestFixture]
internal class ResilienceTests
{
    private TestHttpClientFactory? _httpClientFactory;
    private ILogger? _logger;

    [Test]
    public void GivenFormatException_ShouldRetryAndThrowFormatException()
    {
        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<FormatException>(async () => await sut.CallCzerchNationalBankApi<FormatException>(() => Task.FromException<FormatException>(new FormatException("Test"))));
        sut.ExecutionCounter.Should().Be(2);
    }


    [Test]
    public void GivenHttpRequestException_ShouldRetryAndThrowHttpRequestException()
    {
        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<HttpRequestException>(async () => await sut.CallCzerchNationalBankApi<HttpRequestException>(() => Task.FromException<HttpRequestException>(new HttpRequestException("Test"))));
        sut.ExecutionCounter.Should().Be(2);
    }

    [Test]
    public void GivenException_ShouldNotRetryAndThrowException()
    {
        // act & assert
        var sut = CreateSut();
        var exception = Assert.ThrowsAsync<Exception>(async () => await sut.CallCzerchNationalBankApi<Exception>(() => Task.FromException<Exception>(new Exception("Test"))));
        sut.ExecutionCounter.Should().Be(0);
    }

    private CzechNationalBankRepositoryTestDouble CreateSut()
    {
        return new CzechNationalBankRepositoryTestDouble(_httpClientFactory, _logger);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
        _httpClientFactory = new TestHttpClientFactory("http://localhost/");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _httpClientFactory?.Dispose();
    }
}
