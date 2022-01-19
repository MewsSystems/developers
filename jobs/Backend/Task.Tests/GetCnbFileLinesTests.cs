using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task.Tests;

[TestClass]
public class GetCnbFileLinesTests
{
    [TestMethod]
    public void NullFile_ReturnsEmptyCollection()
    {
        var result = ExchangeRateUpdater.CnbFunctions.GetCnbFileLines(null);
        
        Assert.IsTrue(result.IsEmpty);
    }
    
    [TestMethod]
    public void EmptyFile_ReturnsEmptyCollection()
    {
        var result = ExchangeRateUpdater.CnbFunctions.GetCnbFileLines(String.Empty);
        
        Assert.IsTrue(result.IsEmpty);
    }
    
    [TestMethod]
    public void ValidFileWithOneLine_IsInCollection()
    {
        var file = "18.01.2022 #12\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|15,427";
        
        var result = ExchangeRateUpdater.CnbFunctions.GetCnbFileLines(file);
        
        Assert.IsTrue(result.Count == 1);
        Assert.AreEqual("Austrálie|dolar|1|AUD|15,427", result[0]);
    }
}