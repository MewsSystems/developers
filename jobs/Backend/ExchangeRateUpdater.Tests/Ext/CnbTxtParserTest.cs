using ExchangeRateUpdater.Model;
using JetBrains.Annotations;
using Xunit;

namespace ExchangeRateUpdater.Tests.Ext;

[TestSubject(typeof(CnbTxtParser))]
public class CnbTxtParserTest
{

    [Fact]
    public void CanParseResponse()
    {
        var response = @"15.07.2024 #135
                       země|měna|množství|kód|kurz
                       Austrálie|dolar|1|AUD|15,800
                       Brazílie|real|1|BRL|4,283
                       ";

        var result = CnbTxtParser.ParseResponse(response);
        Assert.Equal(2, result.Count);
    }
}