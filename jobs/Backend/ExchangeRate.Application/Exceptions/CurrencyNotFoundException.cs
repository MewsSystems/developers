namespace ExchangeRate.Application.Exceptions
{
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException(List<string> missingCurrencies)
        : base($"The following currencies are not available: {string.Join(", ", missingCurrencies)}")
        {
        }
    }
}
