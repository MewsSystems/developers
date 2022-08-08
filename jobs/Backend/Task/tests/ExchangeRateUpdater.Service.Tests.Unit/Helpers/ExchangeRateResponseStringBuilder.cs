namespace ExchangeRateUpdater.Service.Tests.Unit.Helpers;

internal class ExchangeRateResponseStringBuilder
{
    private string _dateLine;
    private string _columnNamesLine;
    private readonly List<string> _exchangeRateLines;
    
    public ExchangeRateResponseStringBuilder ()
    {
        _dateLine         = "";
        _columnNamesLine    = "";
        _exchangeRateLines = new List<string>();
    }

    public ExchangeRateResponseStringBuilder WithDateLine(string dateInfo)
    {
        _dateLine = dateInfo;
        return this;
    }

    public ExchangeRateResponseStringBuilder WithColumnInfo(string columnNamesInfo)
    {
        _columnNamesLine = columnNamesInfo;
        return this;
    }

    public ExchangeRateResponseStringBuilder WithExchangeRateInfo(string exchangeRateInfo)
    {
        _exchangeRateLines.Add(exchangeRateInfo);
        return this;
    }

    public IReadOnlyList<string> Build()
    {
        var responseLines = new List<string>();

        if (!string.IsNullOrEmpty(_dateLine))
        {
            responseLines.Add(_dateLine);
        }

        if (!string.IsNullOrEmpty(_columnNamesLine))
        {
            responseLines.Add(_columnNamesLine);
        }

        responseLines.AddRange(_exchangeRateLines);

        return responseLines;
    }
}