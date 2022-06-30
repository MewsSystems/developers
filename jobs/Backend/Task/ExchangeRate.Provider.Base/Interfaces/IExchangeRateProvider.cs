namespace ExchangeRate.Provider.Base.Interfaces;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate.Models.ExchangeRate>> GetExchangeRates();
}