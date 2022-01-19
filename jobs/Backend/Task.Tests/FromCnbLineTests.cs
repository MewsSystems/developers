using ExchangeRateUpdater;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task.Tests;

[TestClass]
public class FromCnbLineTests
{
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("ljkhlhdslhdas")]
    [DataRow("")]
    public void InvalidInput_ReturnsOptionNone(string input)
    {
        var result = CnbFunctions.FromCnbLine(input);
        
        Assert.IsTrue(result.IsNone);
    }


    [DataTestMethod]
    [DataRow("Austrálie|dolar|1|AUD|15,427")]
    [DataRow("Mexiko|peso|1|MXN|1,055")]
    public void ValidInput_ReturnsOptionSome(string input)
    {
        var result = CnbFunctions.FromCnbLine(input);
        
        Assert.IsTrue(result.IsSome);
    }

    [DataTestMethod]
    [DataRow("Austrálie|dolar|1|AUD|15,427", "AUD", 1, 15.427)]
    [DataRow("Filipíny|peso|100|PHP|41,715", "PHP", 100, 41.715)]
    public void ValidInput_ParsedCorrectly(
        string input,
        string expectedCurrency,
        int expectedAmount,
        double expectedValue)
    {
        var result = CnbFunctions.FromCnbLine(input);

        result.Match(
            data =>
            {
                Assert.AreEqual(expectedCurrency, data.Currency.Code);
                Assert.AreEqual(expectedAmount, data.Amount);
                Assert.AreEqual((decimal) expectedValue, data.Value);
            },
            () => Assert.Fail("Should never get here!"));
    }
}