using System;
using ExchangeRateUpdater.Model;
using JetBrains.Annotations;
using Xunit;

namespace ExchangeRateUpdater.Tests.Ext;

[TestSubject(typeof(CnbFxRow))]
public class CnbFxRowTest
{

    [Fact]
    public void ThrowsIfNot5Parts()
    {
        var testStr = "Brazílie|real|1|BRL";
        Assert.Throws<ArgumentException>(() => CnbFxRow.FromSeparatedString(testStr, '|'));
    }


    [Fact]
    public void ParsesDelimitedString()
    {
        var testStr = "Brazílie|real|1|BRL|4,283";
        var result = CnbFxRow.FromSeparatedString(testStr, '|');
        
        Assert.Equal("Brazílie", result.Country);
        Assert.Equal("real", result.CurrencyStr);
        Assert.Equal(1, result.Amount);
        Assert.Equal(new Currency("BRL"), result.Currency);
        Assert.Equal(4.283, (double) result.Rate);
    }
}