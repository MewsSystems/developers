using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Xml;
using ExchangeRateUpdater.Support;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateProviderTests
{
  private Mock<HttpClientHandler> _clientHandler;
  private ExchangeRateProvider _sut;
  
  [SetUp]
  public void BeforeEach()
  {
    _clientHandler = new Mock<HttpClientHandler>();
    _sut = new ExchangeRateProvider(new Currency("CZK"), new HttpClient(_clientHandler.Object) {
      BaseAddress = new Uri("Http://example.local")
    }, new ExchangeRateRequestFactory(), new Parser());
  }

  [Test]
  public void GetExchangeRates_fetches_all_matching_rates()
  {
    // arrange
    var currencies = new[] {
      new Currency("USD"),
      new Currency("PHP")
    };
    var content = new StringContent(File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Fixtures", "Rates.xml")));
    _clientHandler
      .Protected()
      .Setup<HttpResponseMessage>("Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Returns(new HttpResponseMessage(HttpStatusCode.OK) {
        Content = content
      });
    
    // act
    var result = _sut.GetExchangeRates(currencies);

    // assert
    Assert.AreEqual(2, result.Count());
  }

  [Test]
  public void Unmatched_input_currencies_are_ignored()
  {
    // arrange
    var currencies = new[] {
      new Currency("USD"),
      new Currency("PHP"),
      new Currency("XYZ")
    };
    var content = new StringContent(File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Fixtures", "Rates.xml")));
    _clientHandler
      .Protected()
      .Setup<HttpResponseMessage>("Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Returns(new HttpResponseMessage(HttpStatusCode.OK) {
        Content = content
      });
    
    // act
    var result = _sut.GetExchangeRates(currencies);

    // assert
    Assert.AreEqual(2, result.Count());
  }
  
  [Test]
  public void Exception_is_thrown_when_call_completes_without_success_status()
  {
    // arrange
    var currencies = new[] {
      new Currency("USD"),
      new Currency("PHP"),
      new Currency("XYZ")
    };
    _clientHandler
      .Protected()
      .Setup<HttpResponseMessage>("Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Returns(new HttpResponseMessage(HttpStatusCode.NotFound));
    
    // act
    Action action = () => _sut.GetExchangeRates(currencies); 

    // assert
    Assert.Throws<Exception>(() => action());
  }

  [Test]
  public void Exception_is_thrown_when_response_cannot_be_parsed()
  {
    // arrange
    var currencies = new[] {
      new Currency("USD"),
      new Currency("PHP")
    };
    _clientHandler
      .Protected()
      .Setup<HttpResponseMessage>("Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Returns(new HttpResponseMessage(HttpStatusCode.OK) {
        Content = new StringContent(string.Empty)
      });
    
    // act
    Action action = () => _sut.GetExchangeRates(currencies); 

    // assert
    Assert.Throws<XmlException>(() => action());
  }
}