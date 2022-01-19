using ExchangeRateUpdater;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task.Tests;

[TestClass]
public class CreateExchangeRateTests
{
    [TestMethod]
    public void NullArguments_ReturnsOptionNone()
    {
        var rate = CnbFunctions.CreateExchangeRate(null, null);
        
        Assert.IsTrue(rate.IsNone);
    }
    
    [TestMethod]
    public void SourceAndTargetCulture_SetCorrectly()
    {
        var currency = "USD";
        var rate = CnbFunctions.CreateExchangeRate(
            new Types.CnbCurrencyData(1, new Currency(currency), 20m),
            new Currency(currency));
        
        Assert.IsTrue(rate.IsSome);

        rate.Match(er =>
            {
                Assert.AreEqual(currency, er.SourceCurrency.Code);
                Assert.AreEqual("CZK", er.TargetCurrency.Code);
            },
            () => Assert.Fail("Should not get here!"));
    }
}