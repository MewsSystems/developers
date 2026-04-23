namespace ExchangeRateUpdater.Domain.Model.Cnb.Rq
{
    public class CnbExchangeRatesRqModel
    {
        public CnbExchangeRatesRqModel(string language)
        {
            Lang = language;
        }
        public string Lang { get; set; }
    }
}
