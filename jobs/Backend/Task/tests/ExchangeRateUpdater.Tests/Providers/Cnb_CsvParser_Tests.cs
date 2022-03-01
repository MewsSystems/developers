using System.IO;
using System.Linq;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Providers.Cnb;
using Xunit;

namespace ExchangeRateUpdater.Tests.Providers;

public class Cnb_CsvParser_Tests
{
    [Fact]
    public void Should_Parse_Cnb_ExchangeRates()
    {
        using FileStream stream = new FileStream(@".\Resources\Cnb_Rates.csv", FileMode.Open, FileAccess.Read);
        
        CnbCsvParser parser = new CnbCsvParser();
        var parsedRates = parser.ParseExchangeRates(stream).ToList();
            
        Assert.NotEmpty(parsedRates);
        Assert.Contains(parsedRates, x => x.TargetCurrency.Equals(new Domain.Currency("CZK")));
    }
}