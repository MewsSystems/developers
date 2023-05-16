using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using ExchangeRateUpdater.Client.Client;
using ExchangeRateUpdater.Client.Contracts;
using ExchangeRateUpdater.Client.Exceptions;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateUpdater.Client.Tests;

public class ExchangeRateProviderTests
{
    private readonly Fixture _fixture = new();
    
    private readonly Mock<IProviderClient> _providerclientMock;
    private readonly ExchangeRateProvider _sut;
    
    public ExchangeRateProviderTests()
    {
        _providerclientMock = new Mock<IProviderClient>();
        _sut = new ExchangeRateProvider(_providerclientMock.Object);
    }
    
    [Fact]
    public async Task GetAsync_Returns_LatestExchangeRates_WhenDateSpecified()
    {
        // arrange
        var param = new List<Currency>(0);
        var expectedPairs = _fixture.CreateMany<ExchangeRatePair>();
        _providerclientMock.Setup(x => x.GetAsync(It.Is<DateTime?>(p => p == null))).ReturnsAsync(expectedPairs);
        
        // act
        var actual = _sut.GetExchangeRates(param);

        // assert
        Assert.Empty(actual);
    }
    
    [Fact]
    public async Task GetAsync_Throws_ExpectedException_ReceivingBadRequest()
    {
        // arrange
        var param = new List<Currency>
        {
            new("EUR")
        };

        var expectedPairs = new List<ExchangeRatePair>
        {
            new("EMU", "euro", 1m, "EUR", 23.5m),
            _fixture.Create<ExchangeRatePair>(),
            _fixture.Create<ExchangeRatePair>(),
            _fixture.Create<ExchangeRatePair>(),
            _fixture.Create<ExchangeRatePair>()
        };
        _providerclientMock.Setup(x => x.GetAsync(It.Is<DateTime?>(p => p == null))).ReturnsAsync(expectedPairs);
        
        // act
        var actual = _sut.GetExchangeRates(param);

        // assert
        Assert.NotEmpty(actual);
        Assert.Equal(1, actual.Count());
        var actualEur = actual.First();
        Assert.Equal(new Currency("EUR"), actualEur.SourceCurrency);
        Assert.Equal(new Currency("CZK"), actualEur.TargetCurrency);
        Assert.Equal(23.5m, actualEur.Value);

    }
}