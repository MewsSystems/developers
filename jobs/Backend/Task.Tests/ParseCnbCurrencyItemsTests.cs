using ExchangeRateUpdater;
using LanguageExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task.Tests;

[TestClass]
public class ParseCnbCurrencyItemsTests
{
    [TestMethod]
    public void EmptyCollection_ReturnsEmptyCollection()
    {
        var input = Seq<string>.Empty;

        var result = CnbFunctions.ParseCnbCurrencyItems(input);
        
        Assert.IsTrue(result.IsEmpty);
    }

    [TestMethod]
    public void CorrectItem_ParsesToCnbCurrencyData()
    {
        var input = Prelude.Seq(new[] { "Austrálie|dolar|1|AUD|15,427" });
        
        var result = CnbFunctions.ParseCnbCurrencyItems(input);

        var item = result.First();
        
        Assert.AreEqual("AUD", item.Currency.Code);
        Assert.AreEqual(1, item.Amount);
        Assert.AreEqual(15.427m, item.Value);
    }
    
    [TestMethod]
    public void NonsenseItems_ReturnsEmptyCollection()
    {
        var input = Prelude.Seq(new[] { "dadsa dfdvzc sa" });
        
        var result = CnbFunctions.ParseCnbCurrencyItems(input);

        Assert.IsTrue(result.IsEmpty);
    }
}