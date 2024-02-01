namespace Mews.ExchangeRate.Domain;
public interface IRetrieveExchangeRatesFromSource
{
    Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync();
}
