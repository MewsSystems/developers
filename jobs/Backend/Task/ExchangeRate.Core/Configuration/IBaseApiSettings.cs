namespace ExchangeRate.Datalayer.Configuration
{
    public interface IBaseApiSettings
    {
        string CultureInfo { get; set; }
        int CurrencyIndex { get; set; }
        string DailyFileUri { get; set; }
        int RateIndex { get; set; }
        string SourceCurrency { get; set; }
    }
}