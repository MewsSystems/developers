namespace ExchangeRateUpdater.Application.Models
{
    public class CurrencyModel
    {
        public CurrencyModel(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
