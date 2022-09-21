using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Tests.Mocks;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateParserTests
{
    private readonly IExchangeRateParser _parser = new ExchangeRateParser();

    [Fact]
    public async Task ParseAsync_EmptyStream_EmptyResult()
    {
        var ms = new MemoryStream();

        var result = await _parser.ParseAsync(ms);
        
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task ParseAsync_DataWithoutHeaders_EmptyResult()
    {
        var ms = new MemoryStream();
        var sw = new StreamWriter(ms);
        await sw.WriteLineAsync("USA|dolar|1|USD|24,870");
        await sw.WriteLineAsync("Velká Británie|libra|1|GBP|28,210");

        var result = await _parser.ParseAsync(ms);
        
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task ParseAsync_CorrectData_CorrectResults()
    {
        var mock = new ExchangeRateLoaderMock();
        var stream = await mock.ReadAsync();

        var result = await _parser.ParseAsync(stream);
        
        Assert.Empty(result);
    }
}