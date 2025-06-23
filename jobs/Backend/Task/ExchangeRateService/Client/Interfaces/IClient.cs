using ExchangeRateModel;

namespace ExchangeRateService.Client.Interfaces;

/// <summary>
/// Represents a client for communicating with the source of the exchange rate
/// </summary>
public interface IClient
{
    /// <summary>
    /// Target currency of the client it tries to get the rate
    /// </summary>
    Currency TargetCurrency { get; }
    
    /// <summary>
    /// Gets exchange rates from the source for the given currencies
    /// </summary>
    /// <param name="currencies">A list of source currencies</param>
    /// <param name="date">Date of the rate</param>
    /// <returns>A list of exchange rates between currencies and the target currency<see cref="TargetCurrency"/></returns>
    Task<IList<ExchangeRate>> GetExchangeRates(IList<Currency> currencies, DateTime date);
}