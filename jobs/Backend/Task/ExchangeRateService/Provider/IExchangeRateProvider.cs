using ExchangeRateModel;

namespace ExchangeRateService.Provider;

/// <summary>
/// Represents a provider that get exchange rate information
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Gets exchange rate for the selected currency and date
    /// </summary>
    /// <param name="currency">Currency for which we want to know the rate</param>
    /// <param name="date">Date for the exchange rate</param>
    /// <returns>Exchange rate for the currency and the provider currency</returns>
    Task<ExchangeRate> GetExchangeRate(Currency currency, DateTime date);
    
    /// <summary>
    /// Gets exchange rate for the selected currencies and date
    /// </summary>
    /// <param name="currencies">A list of currencies for which we want to know the rate</param>
    /// <param name="date">Date for the exchange rate</param>
    /// <returns>A list of exchange rates for the currencies and the provider currency</returns>
    Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date);

}