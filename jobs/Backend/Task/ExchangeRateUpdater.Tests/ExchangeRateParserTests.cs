using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateParserTests
{
    private readonly ExchangeRateParser _sut;

    public ExchangeRateParserTests()
    {
        _sut = new ExchangeRateParser();
    }
    
    [Fact]
    public async Task SkipsHeaderLines()
    {
        using var stream = GetStream(@"Austrálie|dolar|1|AUD|16,862
Brazílie|real|1|BRL|4,681
Bulharsko|lev|1|BGN|12,505");

        var lines = await _sut.ParseExchangeRateList(stream);
        
        Assert.Single(lines);
        Assert.Equal("BGN", lines.First().CurrencyCode);
    }

    [Fact]
    public async Task CorrectlyParsesNumbers()
    {
        using var stream = GetStream(@"15.08.2022 #157
země|měna|množství|kód|kurz
Island|koruna|100|ISK|17,434
Izrael|nový šekel|1|ILS|7,347
Japonsko|jen|100|JPY|18,037");

        var lines = await _sut.ParseExchangeRateList(stream);

        var isk = lines.FirstOrDefault(x => x.CurrencyCode == "ISK");

        Assert.NotNull(isk);
        Assert.Equal(17.434M, isk.ExchangeRate);
        Assert.Equal(100M, isk.Amount);
    }

    private Stream GetStream(string text)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(text));
    }
}