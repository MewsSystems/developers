using System.IO;
using ExchangeRateUpdater.Support;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
  public class Tests
  {
    private Parser _sut;

    [SetUp]
    public void Setup()
    {
      _sut = new Parser();
    }

    [Test]
    public void Can_parse_a_valid_response()
    {
      // arrange
      var content = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Fixtures", "Rates.xml"));
      
      // act
      var result = _sut.Parse(content);
      
      // assert
      Assert.AreEqual(6, result.Count);
      CollectionAssert.AreEquivalent(new [] {
        new CZKRate("AUD", 16.614m),
        new CZKRate("GBP", 28.843m),
        new CZKRate("USD", 23.798m),
        new CZKRate("BRL", 4.644m),
        new CZKRate("PHP", 0.42755m),
        new CZKRate("HKD", 3.032m)
      }, result);
    }
  }
}