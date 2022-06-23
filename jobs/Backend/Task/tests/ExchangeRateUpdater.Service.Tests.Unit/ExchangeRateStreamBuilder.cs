namespace ExchangeRateUpdater.Service.Tests.Unit;

public class ExchangeRateStreamBuilder
{
    private string _title;
    private string _columnInfo;
    private readonly List<string> _exchangeRates;
    
    public ExchangeRateStreamBuilder ()
    {
        _title         = "";
        _columnInfo    = "";
        _exchangeRates = new List<string>();
    }

    public ExchangeRateStreamBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ExchangeRateStreamBuilder WithColumnInfo(string columnInfo)
    {
        _columnInfo = columnInfo;
        return this;
    }

    public ExchangeRateStreamBuilder WithExchangeRate(string newLine)
    {
        _exchangeRates.Add(newLine);
        return this;
    }

    public Stream Build()
    {
        var stream = new MemoryStream();
        var sw     = new StreamWriter(stream);
        sw.WriteLine(_title);
        sw.WriteLine(_columnInfo);

        foreach (var exchangeRate in _exchangeRates)
            sw.WriteLine(exchangeRate);

        sw.Flush();
        stream.Position = 0;

        return stream;
    }
}