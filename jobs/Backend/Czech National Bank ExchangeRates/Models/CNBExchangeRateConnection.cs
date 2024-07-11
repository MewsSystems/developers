namespace Czech_National_Bank_ExchangeRates.Models
{
    public interface ICNBExchangeRateConnection
    {
        string Url { get; set; }
    }

    public class CNBExchangeRateConnection : ICNBExchangeRateConnection
    {
        public string Url { get; set; }
    }
}
