using ExchangeRateUpdater.Helpers;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class UrlFormatterTests
{
    [Test]
    public void UrlFormatter_noParameters_sameUrlAsInput()
    {
        var url = "http://urlToEndpoint";
        var formatted = UrlFormatter.Construct(DateTime.UtcNow, url, null);
        Assert.AreEqual(url, formatted);
    }

    [Test]
    public void UrlFormatter_withMacroParameters_macrosReplaced()
    {
        var dateTime = new DateTime(2021, 11, 28);
        var url = "http://urlToEndpoint";
        var parameters = "year=%YEAR%&month=%MONTH%&day=%DAY%";
        var expected = $"{url}?year=2021&month=11&day=28";

        var formatted = UrlFormatter.Construct(dateTime, url, parameters);

        Assert.AreEqual(expected, formatted);
    }
}