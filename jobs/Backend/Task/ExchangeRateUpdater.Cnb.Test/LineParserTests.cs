using System;

namespace ExchangeRateUpdater.Cnb.Test;

[TestFixture]
public class LineParserTests
{
    public static object[] CorrectCases = new object[]{
        new object[] { "Brazil|real|1|BRL|4.625", "BRL", 4.625M },
        new object[] { "Hungary|forint|100|HUF|6.414", "HUF", 0.06414M }
    };

    [Test]
    [TestCaseSource(nameof(CorrectCases))]
    public static void WhenCorrectDataPassed_CorrectResultReturned(string input, string expectedSource, decimal expectedRate)
    {
        var result = CnbRateProvider.ParseLine(input);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => 
        {
            Assert.That(result.TargetCurrency.Code, Is.EqualTo("CZK"));
            Assert.That(result.SourceCurrency.Code, Is.EqualTo(expectedSource));
            Assert.That(result.Value, Is.EqualTo(expectedRate));
        });
    }

    [Test]
    [TestCase("")]
    [TestCase("no separated fields")]
    [TestCase("missing field|test|1234")]
    [TestCase("bad amount|test|xxxxxx|XYZ|12.34")]
    [TestCase("bad rate|test|12.34|XYZ|xxxx")]
    [TestCase("bad decimal sign|test|12,34|XYZ|56.78")]
    [TestCase("bad decimal sign|test|12.34|XYZ|56,78")]
    public static void WhenIncorrectFormatPassed_ThrowsFormatException(string input)
    {
        Assert.That(() => CnbRateProvider.ParseLine(input), Throws.ArgumentException);
    }
}
