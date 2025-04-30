namespace ExchangeRateProviderAPI_PaolaRojas.Models.Requests
{
    public class CurrencyRequest
    {
        public required IEnumerable<Currency> Currencies { get; set; }
    }
}
