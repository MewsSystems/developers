namespace ExchangeRateUpdater.Models;

/// <summary>
///     The response item of the daily exchange rates endpoint
/// </summary>
/// <param name="ValidFor">The reference date</param>
/// <param name="Order"></param>
/// <param name="Country">The country of the currency</param>
/// <param name="Currency">The description of the currency</param>
/// <param name="Amount"></param>
/// <param name="CurrencyCode">The code of the currency</param>
/// <param name="Rate">The exchange rate</param>
public record GetDailyExchangeRatesResponseItem(
    DateOnly ValidFor,
    int Order,
    string Country,
    string Currency,
    int Amount,
    string CurrencyCode,
    decimal Rate);